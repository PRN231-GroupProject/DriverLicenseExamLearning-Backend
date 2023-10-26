using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.HandleError;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{

    public class ExamService : IExamService
    {
        private readonly IClaimsService _claimService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public ExamService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimService = claimService;
        }

        public async Task<bool> ChangeStatusExam(int examID, string status)
        {
            var exam = _unitOfWork.Repository<Exam>().Where(x => x.ExamId == examID).FirstOrDefault();
            if (exam == null)
            {
                throw new CrudException<string>(System.Net.HttpStatusCode.NotFound, "ExamID not found in the system", "");
            }

            exam.Status = status;

            await _unitOfWork.Repository<Exam>().Update(exam, examID);
            _unitOfWork.Commit();
            return true;
        }



        public async Task<bool> CreateExam(CreateNewExamRequest create)
        {
            if (_unitOfWork.Repository<Exam>().Where(x => x.ExamName == create.ExamName).FirstOrDefault() is not null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, $"Have this {create.ExamName} in ExamBank before");
            }
            var newExam = _mapper.Map<Exam>(create);
            newExam.ExamDate = DateTime.UtcNow;
            await _unitOfWork.Repository<Exam>().CreateAsync(newExam);
            _unitOfWork.Commit();
            Exam exam = _unitOfWork.Repository<Exam>().Where(x => x.ExamName == newExam.ExamName).FirstOrDefault();
            int examID = exam.ExamId;
            int licenseID = (int)exam.LicenseId;
            List<string> errorHandle = new List<string>();
            foreach (var item in create.QuestionID)
            {

                if (_unitOfWork.Repository<Question>().Where(x => x.QuestionId == item).FirstOrDefault().LicenseType != licenseID)
                {
                    errorHandle.Add($"This {item} not have the same type with the exam");
                    continue;
                }
                ExamQuestion examCreate = new ExamQuestion()
                {
                    ExamId = examID,
                    QuestionId = item,
                    Status = "Active"
                };

                await _unitOfWork.Repository<ExamQuestion>().CreateAsync(examCreate);
                _unitOfWork.Commit();
            }
            if (errorHandle.Count > 0)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Error when create new exam" + string.Join(Environment.NewLine, errorHandle));
            }
            return true;

        }

        public async Task<string> DoingQuizByNonUser(AnswerByMemberRequest answer)
        {
            int rightAnswer = 0;
            int wrongAnswer = 0;
            int total = _unitOfWork.Repository<ExamQuestion>().Where(x => x.QuestionId == answer.QuizID).Count();
            foreach (var request in answer.answerDetails)
            {
                var checkTrue = _unitOfWork.Repository<Question>().Where(x => x.QuestionId == request.QuestionID && x.Answer == request.Answer).FirstOrDefault();
                if (checkTrue is null)
                {
                    bool checkParalysis = (bool)_unitOfWork.Repository<Question>().Where(x => x.QuestionId == request.QuestionID ).FirstOrDefault().IsParalysisQuestion;
                    if (checkParalysis)
                    {
                        throw new HttpStatusCodeException(HttpStatusCode.Accepted, $"You wrong in Paralaysis Question ");
                    }

                }
                else
                {
                    rightAnswer++;
                }



            }
            return $"{rightAnswer}/{total}";

        }

        public async Task<string> DoingQuiz(AnswerByMemberRequest answer)
        {
            string result = "";
            int checkCustomer = 0;
            checkCustomer = _claimService.GetCurrentUserId;
            if (checkCustomer == 0)
            {
                result = await DoingQuizByNonUser(answer);
            }
            else
            {
                result = await DoingQuizByMember(answer);
            }
            return result;
        }

        public async Task<string> DoingQuizByMember(AnswerByMemberRequest answer)
        {
            //? Doing Quiz Process
            int AttemptNumber = 0;
            int rightAnswer = 0;

            //Check The number of time to this quiz or maybe first time 
            var examResultFind = _unitOfWork.Repository<ExamResult>().Where(x => x.ExamId == answer.QuizID && x.UserId == _claimService.GetCurrentUserId).FirstOrDefault();
            if (examResultFind is null)
            {
                AttemptNumber = 1;
            }
            else
            {
                AttemptNumber += 1;
            }

            ExamResult exam = new ExamResult()
            {
                UserId = _claimService.GetCurrentUserId,
                AttemptNumber = AttemptNumber,
                Date = DateTime.Now,
                ExamId = answer.QuizID,
                Result = "NoUpdateYet"
            };
            await _unitOfWork.Repository<ExamResult>().CreateAsync(exam);
            _unitOfWork.Commit();



            int examResultID = _unitOfWork.Repository<ExamResult>().Where(x => x.Result == "NoUpdateYet").FirstOrDefault().ExamResultId;
            List<int> mark = await RightNumberAnswer(answer.answerDetails, examResultID);

            exam.Result = $"{mark.First()}/{answer.answerDetails.Count()}";

            await _unitOfWork.Repository<ExamResult>().Update(exam, examResultID);
            _unitOfWork.Commit();

            return $"Your Mark: {mark}/{answer.answerDetails.Count()} and Number Of Paralysis Wrong: {mark.Last()}";

        }

        public async Task<IQueryable<ResultExamByCustomerResponse>> GetExamHistory(int licenseTypeID)
        {
            int userID = _claimService.GetCurrentUserId;
            IQueryable<ResultExamByCustomerResponse> results = await QueryFormat.GetHistoryExam(licenseTypeID, userID);
            return results;
        }

        public async Task<IQueryable<ExamGetByLicenseTye>> GetExamListByCustomer()
        {
            var examQuery = await QueryFormat.QueryExamFollowLisenceTypeByMember();
            return examQuery;
        }

        public async Task<IQueryable<ExamQueryGeneralResponse>> GetExamQuery()
        {
            var examQuery = await QueryFormat.QueryExamFollowLisenceTypeGetByStaff();
            return examQuery;
        }



        public async Task<bool> ModifiedExam(uint quizID, ModifyQuizRequest modify)
        {
            var checkExam = _unitOfWork.Repository<Exam>().Where(x => x.ExamId == quizID).FirstOrDefault();
            if (checkExam is null)
            {
                return false;
            }
            if (modify.RemoveFromQuiz is not null)
            {
                foreach (var item in modify.RemoveFromQuiz)
                {
                    ExamQuestion examQuestion = _unitOfWork.Repository<ExamQuestion>().Where(x => x.ExamId == quizID && x.QuestionId == item).FirstOrDefault();
                    if (examQuestion is not null)
                    {
                        examQuestion.Status = "Delete";
                    }

                }

            }
            if (modify.AddToQuiz is not null)
            {
                foreach (var item in modify.AddToQuiz)
                {
                    ExamQuestion create = new ExamQuestion()
                    {
                        ExamId = (int?)quizID,
                        Status = "Active",
                        QuestionId = item
                    };
                    await _unitOfWork.Repository<ExamQuestion>().CreateAsync(create);
                    _unitOfWork.Commit();


                }

            }

            return true;



        }

        private async Task<List<int>> RightNumberAnswer(List<AnswerDetailMemberRequest> requests, int examResultID)
        {
            List<int> markOut = new List<int>();
            int rightAnswer = 0;
            int paralysisAnswerWrong = 0;
            foreach (var request in requests)
            {
                var checkTrue = _unitOfWork.Repository<Question>().Where(x => x.QuestionId == request.QuestionID && x.Answer == request.Answer).FirstOrDefault();
                if (checkTrue is null)
                {
                    if ((bool)checkTrue.IsParalysisQuestion)
                    {
                        paralysisAnswerWrong++;
                        ExamResultDetail detail = new ExamResultDetail()
                        {
                            QuestionId = request.QuestionID,
                            WrongAnswer = request.Answer,
                            ExamResultId = examResultID
                        };
                        await _unitOfWork.Repository<ExamResultDetail>().CreateAsync(detail);
                        _unitOfWork.Commit();
                    }
                    else
                    {
                        {
                            ExamResultDetail detail = new ExamResultDetail()
                            {
                                QuestionId = request.QuestionID,
                                WrongAnswer = request.Answer,
                                ExamResultId = examResultID
                            };
                            await _unitOfWork.Repository<ExamResultDetail>().CreateAsync(detail);
                            _unitOfWork.Commit();

                        }
                    }
                }
                else
                {
                    rightAnswer++;
                }

                markOut.Add(rightAnswer);
                markOut.Add(paralysisAnswerWrong);


            }
            return markOut;
        }

    }
}
