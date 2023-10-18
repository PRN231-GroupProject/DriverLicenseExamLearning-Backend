﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using FirebaseAdmin.Messaging;

//using DriverLicenseExamLearning_Service.ServiceBase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/questionbank")]
    public class QuestionBankController : ODataController
    {

        private readonly IQuestionBankService _service;
        public QuestionBankController(IQuestionBankService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add List Question to Bank Question
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddMultipleQuestion([FromBody]List<AddQuestionRequest> quizRequests)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            bool checkFalse = await _service.AddQuizRequests(quizRequests);

            if (!checkFalse)
            {
                return BadRequest (new {
                    msg = "Have Error"
                });
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
