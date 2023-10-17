using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{

    public class ExamService : IExamService
    {
        private readonly IClaimsService _claimService;
        private readonly IMapper _mapper; 
        private readonly IUnitOfWork _unitOfWork;


        public ExamService(IUnitOfWork unitOfWork,IMapper mapper, IClaimsService claimService)
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

        //public Task<bool> CreateNewExam(CreateNewExamRequest create)
        //{
        //    var newExam = _mapper.Map<Exam>(create);
        //    newExam.ExamDate = DateTime.Now;
        //    _unitOfWork.Repository<Exam>().CreateAsync(newExam);
        //    Exam exam = _unitOfWork.Repository<Exam>().Where(x => x.ExamDate == newExam.ExamDate).FirstOrDefault();
        //    int examID = exam.ExamId;
        //    int licenseID = (int)exam.LicenseId;
        //    foreach (var item in create.QuestionID)
        //    {

        //        if (_unitOfWork.Repository<Question>().Where(x => x.QuestionId == item).FirstOrDefault().)
        //        {

        //        }
        //        ExamQuestion exam = new ExamQuestion()
        //        {
        //            ExamId = ExamId,
        //            QuestionId = item
        //           ,
        //            Status = "Active"
        //        };

        //        _unitOfWork.Repository<ExamQuestion>().CreateAsync(exam);
        //        _unitOfWork.Commit();
        //    }
        //    return true;

        //}

        public Task<int> DoingQuiz(AnswerByMemberRequest answer)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ResultExamByCustomerResponse>> GetExamHistory(int licenseTypeID)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ExamGetByMemberResponse>> GetExamListByCustomer(int licenseTypeID)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ExamQueryGeneralResponse>> GetExamQuery()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ModifiedExam(ModifyQuizRequest modify)
        {
            throw new NotImplementedException();
        }
    }
}
