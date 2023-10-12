using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IExamService
    {
        Task<bool> CreateNewExam(CreateNewExamRequest create);
        Task<bool> ModifiedExam(ModifyQuizRequest modify);

        Task<bool> ChangeStatusExam(int examID, string status);

        Task<IQueryable<ExamQueryGeneralResponse>> GetExamQuery();

    }
}
