using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/exam")]
    public class ExamController : ODataController
    {

        private readonly IExamService _examService;

        public ExamController(IExamService examService)
        {
            _examService = examService;
        }

        [EnableQuery]
        [HttpGet("GetQuizByStaff")]
        public async Task<ActionResult<IQueryable<ExamQueryGeneralResponse>>> GetQuiz()
        {
           var  quiz = await _examService.GetExamQuery();
            return Ok(quiz);
        }

        [EnableQuery]
        [HttpGet("GetQuizByMember")]
        public async Task<ActionResult<IQueryable<ExamGetByMemberResponse>>> GetQuiz(int licenseTypeID)
        {
            var quiz = await _examService.GetExamListByCustomer(licenseTypeID);
            return Ok(quiz);

        }

        [HttpPost("DoingQuiz")]
        public async Task<ActionResult> DoingQuiz([FromBody]AnswerByMemberRequest answer)
        {
            string result = await _examService.DoingQuiz(answer);   
            return Ok(result);  
        }


        [HttpGet("GetQuizHistoryByCustomer")]
        [EnableQuery]
        public async Task<ActionResult<IQueryable>> GetQuizHistory(int licenseTypeID)
        {
            var quizHistory =await _examService.GetExamHistory(licenseTypeID);
            return Ok(quizHistory);
        }


        [HttpPut("Update")]
        public async Task<ActionResult> Update(uint quizID,[FromBody]ModifyQuizRequest request)
        {
              await _examService.ModifiedExam(quizID, request);
            return Ok(new
            {
                message = "Update Quiz Sucessfully"
            });
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody]CreateNewExamRequest create)
        {
            await _examService.CreateExam(create);
            return Ok(
                new
                {
                    message = "Create Exam Sucessfully"
                });
        }









    }
}
