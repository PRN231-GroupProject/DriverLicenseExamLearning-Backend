using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData;
using Swashbuckle.AspNetCore.Annotations;

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

        [Authorize(Roles = RoleNames.Staff)]
        [SwaggerOperation(Summary = $"[Role : {RoleNames.Staff}][Description:Get All Quiz]")]
        [EnableQuery]
        [HttpGet("GetQuizByStaff")]
        public async Task<ActionResult<IQueryable<ExamQueryGeneralResponse>>> GetQuiz()
        {
           var  quiz = await _examService.GetExamQuery();
            if(quiz is null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

       
        [EnableQuery]
        [HttpGet("GetQuizByMember")]
        public async Task<ActionResult<IQueryable<ExamGetByLicenseType>>> GetQuizByMemeber()
        {
            
            var quiz = await _examService.GetExamListByCustomer();
            if(quiz is null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

      
        [HttpPost("DoingQuiz")]
        public async Task<ActionResult> DoingQuiz([FromBody]AnswerByMemberRequest answer)
        {
            string result = await _examService.DoingQuiz(answer);   
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);  
        }

        [Authorize(Roles = RoleNames.Member)]
        [SwaggerOperation(Summary = $"[Role : {RoleNames.Member}][Description: View Quiz Customer have been done before]")]
        [HttpGet("GetQuizHistoryByCustomer")]
        [EnableQuery]
        public async Task<ActionResult<IQueryable>> GetQuizHistory(int licenseTypeID)
        {
            var quizHistory =await _examService.GetExamHistory(licenseTypeID);
            if(quizHistory is null)
            {
                return NotFound();
            }
            return Ok(quizHistory);
        }

        [Authorize(Roles = RoleNames.Staff)]
        [SwaggerOperation(Summary = $"[Role : {RoleNames.Staff}][Description:Modify Quiz with different question]")]
        [HttpPut("Update")]
        public async Task<ActionResult> Update(uint quizID,[FromBody]ModifyQuizRequest request)
        {
              await _examService.ModifiedExam(quizID, request);
            return Ok(new
            {
                message = "Update Quiz Sucessfully"
            });
        }

        [Authorize(Roles = RoleNames.Staff)]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody]CreateNewExamRequest create)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            await _examService.CreateExam(create);
            return Ok(
                new
                {
                    message = "Create Exam Sucessfully"
                });
        }









    }
}
