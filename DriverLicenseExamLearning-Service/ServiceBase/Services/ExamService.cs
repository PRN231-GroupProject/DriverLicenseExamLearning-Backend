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

        public async Task<MarkResultResponse> DoingQuizByNonUser(AnswerByMemberRequest answer)
        {
            int rightAnswer = 0;
            int wrongAnswer = 0;
            int paralysisAnswer = 0;
            int total = _unitOfWork.Repository<ExamQuestion>().Where(x => x.ExamId == answer.QuizID).Count();
            foreach (var request in answer.answerDetails)
            {
                var checkTrue = _unitOfWork.Repository<Question>().Where(x => x.QuestionId == request.QuestionID && x.Answer == request.Answer).FirstOrDefault();
                if (checkTrue is null)
                {
                    bool checkParalysis = (bool)_unitOfWork.Repository<Question>().Where(x => x.QuestionId == request.QuestionID).FirstOrDefault().IsParalysisQuestion;
                    if (checkParalysis)
                    {
                        paralysisAnswer++;
                    }
                    else
                    {
                        wrongAnswer++;
                    }

                }
                else
                {
                    rightAnswer++;
                }



            }

            double examStatusCheck = PercentageInLicenseType((int)await _unitOfWork.Repository<Exam>().Where(x => x.ExamId == answer.QuizID).Select(x => x.LicenseId).FirstOrDefaultAsync());
            string? examCheck;
            if (paralysisAnswer > 0)
            {
                examCheck = "Failed";
            }
            else
            {
                examCheck = rightAnswer / total > examStatusCheck ? "Passed" : "Failed";
            }
            return new MarkResultResponse
            {
                Mark = $"{rightAnswer}/{total}",
                RightAnswer = rightAnswer,
                WrongAnswer = wrongAnswer + paralysisAnswer,
                WrongParalysisAnswer = paralysisAnswer,
                ExamStatus = examCheck,

            };

        }

        public async Task<MarkResultResponse> DoingQuiz(AnswerByMemberRequest answer)
        {
            MarkResultResponse result;
            int checkCustomer = 0;
            checkCustomer = _claimService.GetCurrentUserId;
            int checkInTransaction = await QueryFormat.CheckMemberInBooking(answer.QuizID, checkCustomer);
            if (checkCustomer == 0 || checkInTransaction == 0)
            {
                result = await DoingQuizByNonUser(answer);
            }
            else
            {
                result = await DoingQuizByMember(answer);
            }
            return result;
        }

        public async Task<MarkResultResponse> DoingQuizByMember(AnswerByMemberRequest answer)
        {
            //? Doing Quiz Process
            int AttemptNumber = 0;


            //Check The number of time to this quiz or maybe first time 
            AttemptNumber = _unitOfWork.Repository<ExamResult>().Where(x => x.ExamId == answer.QuizID && x.UserId == _claimService.GetCurrentUserId).Max(x => (int?)x.AttemptNumber) ?? 0 ;
          
            if (AttemptNumber == 0)
            {
                AttemptNumber = 1;
            }
            else
            {

                AttemptNumber++;
            }

            ExamResult exam = new ExamResult()
            {
                UserId = _claimService.GetCurrentUserId,
                AttemptNumber = AttemptNumber,
                Date = DateTime.Now,
                ExamId = answer.QuizID,
                Result = "",
            };
            await _unitOfWork.Repository<ExamResult>().CreateAsync(exam);
            _unitOfWork.Commit();


            int totalQuestion = _unitOfWork.Repository<ExamQuestion>().Where(x => x.ExamId == exam.ExamId).Count();
            ExamResult examResultID = await _unitOfWork.Repository<ExamResult>().Where(x => x.ExamId == exam.ExamId && x.AttemptNumber ==exam.AttemptNumber ).FirstOrDefaultAsync();
            List<int> mark = await RightNumberAnswer(answer.answerDetails, examResultID.ExamResultId);


            examResultID.Result = $"{mark.First()}/{totalQuestion}";
     

            await _unitOfWork.Repository<ExamResult>().Update(examResultID, examResultID.ExamResultId);
            _unitOfWork.Commit();
            double examStatusCheck = PercentageInLicenseType((int)await _unitOfWork.Repository<Exam>().Where(x => x.ExamId == answer.QuizID).Select(x => x.LicenseId).FirstOrDefaultAsync());
            string? examCheck;
            if (mark.Last() > 0)
            {
                examCheck = "Failed";
            }
            else
            {
                examCheck = mark.First() / totalQuestion > examStatusCheck ? "Passed" : "Failed";
            }

            return new MarkResultResponse
            {
                WrongParalysisAnswer = mark.Last(),
                Mark = $"{mark.First()}/{totalQuestion}",
                RightAnswer = mark.First(),
                WrongAnswer = totalQuestion - mark.First(),
                ExamStatus = examCheck

            };
        }

        public async Task<IEnumerable<ExamResultResponse>> GetExamHistory(int licenseTypeID)
        {
            int userID = _claimService.GetCurrentUserId;
            IEnumerable<ExamResultResponse> result =await _unitOfWork.Repository<ExamResult>().Include(x => x.Exam).Where(x => x.UserId == userID && x.Exam.LicenseId == licenseTypeID).GroupBy(x => x.ExamId).Select(group => new ExamResultResponse
            {
                ExamId = group.First().Exam.ExamId,
                ExamName = group.First().Exam.ExamName,
                Details = group.Select(x => new ExamResultDetailResponse
               {
                  
                        AttemptNumber = (int)x.AttemptNumber,
                        Date = (DateTime)x.Date,
                        ExamResultId = x.ExamResultId,
                        Result = x.Result
                   
               }).ToList()
            }).ToListAsync();

            
            return result;
        }



        public async Task<IEnumerable<ExamGetByLicenseType>> GetExamListByCustomer()
        {

            IEnumerable<ExamGetByLicenseType> examQuery = await _unitOfWork.Repository<Exam>().Include(x => x.License).Include(x => x.ExamQuestions).ThenInclude(x => x.Question).GroupBy(x => x.LicenseId).Select(group => new ExamGetByLicenseType
            {
                LicenseId = (int)group.First().LicenseId,
                LicenseName = group.First().ExamName,
                exams = group.Select(x => new ExamGetByMemberResponse
                {
                    ExamDate = (DateTime)x.ExamDate,
                    ExamId = x.ExamId,
                    ExamName = x.ExamName,
                    questions = x.ExamQuestions.Select(eq => new QuestionGetByMemberResponse
                    {

                        QuestionId = eq.Question.QuestionId,
                        Image = eq.Question.Image,
                        Option1 = eq.Question.Option1,
                        Option2 = eq.Question.Option2,
                        Option3 = eq.Question.Option3,
                        Option4 = eq.Question.Option4,
                        Title = eq.Question.Question1
                    }).ToList()

                }).ToList()
            }).ToListAsync();

           
            return examQuery;
        }

        public async Task<IEnumerable<ExamQueryGeneralResponse>> GetExamQuery()
        {
            var examQuery = await _unitOfWork.Repository<Exam>()
                .Include(ex => ex.License)
                .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .GroupBy(x => x.LicenseId)
                .Select(group => new ExamQueryGeneralResponse
                {
                    LicenseTypeId = group.First().License.LicenseTypeId,
                    Name = group.First().License.LicenseName,
                    examQueries =group.Select( ex => new ExamQueryResponse
                    {
                       
                            ExamName = ex.ExamName,
                            Date = ex.ExamDate,
                            ExamId = ex.ExamId,
                            examDetails = ex.ExamQuestions.Select(eq => new ExamDetailResponse
                            {
                                Image = eq.Question.Image,
                                QuestionId = eq.Question.QuestionId,
                                Answer = eq.Question.Answer,
                                Options1 = eq.Question.Option1,
                                Options2 = eq.Question.Option2,
                                Options3 = eq.Question.Option3,
                                Options4 = eq.Question.Option4,
                                Text = eq.Question.Question1
                            }).ToList()
                        
                    }).ToList()
                })
                .ToListAsync();

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
                    ExamQuestion examQuestion = await _unitOfWork.Repository<ExamQuestion>().Where(x => x.ExamId == quizID && x.QuestionId == item).FirstOrDefaultAsync();
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

                    bool checkParalysisQuestion = (bool)_unitOfWork.Repository<Question>().Where(x => x.QuestionId == request.QuestionID).FirstOrDefault().IsParalysisQuestion;
                    if (checkParalysisQuestion)
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
        private double PercentageInLicenseType(int licenseType)
        {
            switch (licenseType)
            {
                case 1: return 0.9;
                case 2: return 0.8;
                case 3: return 0.9;

            }
            throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not Found the License Type");
        }

        public async Task<IEnumerable<ResultExamDetailByCustomerResponse>> GetExamDetailHistory(int examResultId)
        {
            Dictionary<int, ResultExamDetailByCustomerResponse> result = new Dictionary<int, ResultExamDetailByCustomerResponse>();
            List<ExamResultDetail> exam = _unitOfWork.Repository<ExamResultDetail>().Where(x => x.ExamResultId == examResultId).ToList();
            foreach (var item in exam)
            {
                Question question = _unitOfWork.Repository<Question>().Where(x => x.QuestionId == item.QuestionId).FirstOrDefault();
                ResultExamDetailByCustomerResponse resultDetail = new ResultExamDetailByCustomerResponse()
                {
                    Image = question.Image,
                    Option1 = question.Option1,
                    Option2 = question.Option2,
                    Option3 = question.Option3,
                    Option4 = question.Option4,
                    QuestionId = question.QuestionId,
                    RightAnswer = question.Answer,
                    Title = question.Question1,
                    UserAnswer = item.WrongAnswer,

                };
                result[(int)item.QuestionId] = resultDetail;
            }

            int examId  = (int)_unitOfWork.Repository<ExamResult>().Where(x => x.ExamResultId == examResultId).FirstOrDefault().ExamId;
            List<ExamQuestion> questionIDList = _unitOfWork.Repository<ExamQuestion>().Where(x => x.ExamId == examId).ToList();
            List<int> take = new List<int>();
            foreach (var item in questionIDList)
            {
                if (!result.ContainsKey((int)item.QuestionId))
                {
                take.Add((int)item.QuestionId);

                }
            }
            foreach (var questionID in take)
            {
                Question question = _unitOfWork.Repository<Question>().Where(x => x.QuestionId == questionID).FirstOrDefault();
                ResultExamDetailByCustomerResponse resultDetail = new ResultExamDetailByCustomerResponse()
                {
                    Image = question.Image,
                    Option1 = question.Option1,
                    Option2 = question.Option2,
                    Option3 = question.Option3,
                    Option4 = question.Option4,
                    QuestionId = question.QuestionId,
                    RightAnswer = question.Answer,
                    Title = question.Question1,
                    UserAnswer =question.Answer

                };
                result[questionID] = resultDetail;
            }
            List<ResultExamDetailByCustomerResponse> list = new List<ResultExamDetailByCustomerResponse>();
            foreach (var item in result)
            {
                list.Add(item.Value);   
            }

            return list.AsEnumerable();

        }
    }
}
