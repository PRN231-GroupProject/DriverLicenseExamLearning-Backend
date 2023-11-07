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
        // Create New Exam by Staff
        //Task<bool> CreateNewExam(CreateNewExamRequest create);
       //Modify Exam by Staff
        Task<bool> ModifiedExam(uint quizID,ModifyQuizRequest modify);

        // Update Status Exam by Staff
        Task<bool> ChangeStatusExam(int examID, string status);

        //Get All Exam Query by Staff
        Task<IEnumerable<ExamQueryGeneralResponse>> GetExamQuery();

        //Get Quiz by Customer
        Task<IEnumerable<ExamGetByLicenseType>> GetExamListByCustomer();

        //Doing quiz
        Task<MarkResultResponse> DoingQuiz(AnswerByMemberRequest answer); 


        //View Exam History Doing follow licenseType
        Task<IEnumerable<ExamResultResponse>> GetExamHistory(int licenseTypeID);

        Task<IEnumerable<ResultExamDetailByCustomerResponse>> GetExamDetailHistory(int examResultId);
        Task<bool> CreateExam(CreateNewExamRequest request);



       




    }
}
