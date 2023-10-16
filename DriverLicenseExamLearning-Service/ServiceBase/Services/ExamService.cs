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
        private readonly IMapper _mapper; 
        private readonly IUnitOfWork _unitOfWork;


        public ExamService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public Task<bool> CreateNewExam(CreateNewExamRequest create)
        {
            var newExam = _mapper.Map<Exam>(create);
            newExam.ExamDate = DateTime.Now;
            _unitOfWork.Repository<Exam>().CreateAsync(newExam);
            int ExamId = _unitOfWork.Repository<Exam>().Where(x => x.ExamDate == newExam.ExamDate).FirstOrDefault().ExamId;
            foreach (var item in create.QuestionID)
            {
                Question  question = new Question
                {
                    ExamId = ExamId,

                    
                }
            }

        }

        public Task<int> ExamResult(AnswerByMemberRequest answer)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<ExamGetByMemberResponse>> GetExamListByCustomer()
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
