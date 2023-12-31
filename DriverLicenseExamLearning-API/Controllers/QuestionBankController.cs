﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;

//using DriverLicenseExamLearning_Service.ServiceBase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;

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

        [SwaggerOperation(Summary = $"[Role:{RoleNames.Staff}][Description:Using to add many question to QuestionBank]")]
        [Authorize(Roles = RoleNames.Staff)]
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

            return Ok(new
            {
                message = "Add Question To Bank Sucessfully"
            });
        }
        
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<QuestionBankResponse>>> Get()
        {
            IEnumerable<QuestionBankResponse> result = await _service.QuestionBank();
            if(result == null)
            {
                return NotFound(new { 
                    message ="Not Found" });
            }
            return Ok(result);
        }
      
        [Authorize(Roles = RoleNames.Staff)]
        [HttpPut]
        public async Task<IActionResult> Update(int questionID, [FromBody] AddQuestionRequest request)
        {
            await _service.UpdateQuizRequests(questionID, request);

            return Ok(new
            {
                message = "Update Sucessfully"
            });
        }

        [Authorize(Roles = RoleNames.Staff)]
        [HttpPost("Upload-image")]
        public async Task<IActionResult> Upload(IFormFile imgae)
        {
            if(imgae != null)
            {
                FireBaseFile imageInfo =  await FirebaseHelper.UploadFileAsync(imgae,"signage");
                return Ok(new { imageInfo.FileName });
            }
            return BadRequest(new
            {
                message = "Download Failed"
            });

        }
    }
}
