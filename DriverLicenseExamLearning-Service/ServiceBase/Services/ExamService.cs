//using DriverLicenseExamLearning_Data.Entity;
//using DriverLicenseExamLearning_Data.UnitOfWork;
//using DriverLicenseExamLearning_Service.DTOs.Request;
//using DriverLicenseExamLearning_Service.DTOs.Response;
//using DriverLicenseExamLearning_Service.ServiceBase.IServices;
//using DriverLicenseExamLearning_Service.Support.Ultilities;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DriverLicenseExamLearning_Service.ServiceBase.Services
//{

//    public class ExamService : IExamService
//    {
//        private readonly IUnitOfWork _unitOfWork;


//        public ExamService(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }
//        public async Task<bool> ChangeStatusExam(int examID, string status)
//        {
//            Exam check = _unitOfWork.Repository<Exam>().Where(x => x.ExamId == examID).FirstOrDefault();
//            if (check != null)
//            {
//                check.Status = status;
//            }

//            return true;
//        }

//        public async Task<bool> CreateNewExam(CreateNewExamRequest create)
//        {
//            List<string> list = new List<string>();
//            int checkOutOfLiscenseType = 0;
//            Exam exam = new Exam() { ExamDate = DateTime.Now, LicenseTypeId = create.LicenseTypeId, Status = create.Status };
//            await _unitOfWork.Repository<Exam>().CreateAsync(exam);
//            _unitOfWork.Commit();

//            int getExamId = _unitOfWork.Repository<Exam>().Where(x => x.ExamDate == DateTime.Now).FirstOrDefault().ExamId;
//            foreach (var item in create.QuestionID)
//            {
//                if (_unitOfWork.Repository<Question>().Where(x => x.QuestionId == item).FirstOrDefault() is null)
//                {
//                    checkOutOfLiscenseType++;
//                    list.Add($"{item} is not have the same type with this Quiz");
//                    continue;
//                }
//                ExamQuestion examQuestion = new ExamQuestion() { ExamId = getExamId, QuestionId = item, Status = "Active" };
//                await _unitOfWork.Repository<ExamQuestion>().CreateAsync(examQuestion);
//                _unitOfWork.Commit();
//            }
//            if (checkOutOfLiscenseType > 0)
//            {
//                throw new CrudException<List<string>>(System.Net.HttpStatusCode.BadRequest, "Have when add new question to Quiz", list);
//            }

//            return true;



//        }

//        public async Task<IQueryable<ExamQueryGeneralResponse>> GetExamQuery()
//        {
//            IQueryable<ExamQueryGeneralResponse> result = await QueryFormat.QueryExamFollowLisenceType();

//            return result;
//        }

//        public Task<bool> ModifiedExam(ModifyQuizRequest modify)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
