using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    
    public class ExamController : ODataController
    {

        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IQueryable<ExamQueryGeneralResponse>>> GetQuiz()
        {
           var  quiz = await _examService.GetExamQuery();
            return Ok(quiz);
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IQueryable<ExamGetByMemberResponse>>> GetQuiz(int quizID)
        {
            var quiz = await _examService.GetExamListByCustomer(quizID);
            return Ok(quiz);

        }

        [HttpPost]
        public async Task<ActionResult> DoingQuiz(AnswerByMemberRequest answer)
        {
            int result = await _examService.DoingQuiz(answer);   
            return Ok(result);  
        }


        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable>> GetQuizHistory(int licenseTypeID)
        {
            var quizHistory = _examService.GetExamHistory(licenseTypeID);
            return Ok(quizHistory);
        }


        [HttpPost]
        public async Task<ActionResult> Update(ModifyQuizRequest request)
        {
              await _examService.ModifiedExam(request);
            return Ok(new
            {
                message = "Update Quiz Sucessfully"
            });
        }









    }
}
