using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    public class QuestionBankController : ODataController
    {

        private readonly IQuestionBankService _service;
        public QuestionBankController(IQuestionBankService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> AddMultipleQuestion(List<AddQuestionRequest> quizRequests)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            bool checkFalse = await _service.AddQuizRequests(quizRequests);

            if (!checkFalse)
            {
                return BadRequest("Something Wrong");
            }

            return Ok("Add Question To Bank Sucessfully");
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var result = await _service.QuestionBank();
            if(result == null)
            {
                return NotFound("Not Found");
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int questionID, [FromBody] AddQuestionRequest request)
        {
            await _service.UpdateQuizRequests(questionID, request);

            return Ok("Update Sucessfully");
        }

    }
}
