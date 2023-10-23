﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Text.RegularExpressions;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/license_application")]
    public class LicenseApplicationController : ODataController
    {
        private readonly ILicenseApplicationService _licenseApplicationService;

        public LicenseApplicationController(ILicenseApplicationService licenseApplicationService)
        {
            _licenseApplicationService = licenseApplicationService;
        }


        [HttpPost("Submit")]
        public async Task<ActionResult> Submit(SubmitLicenseApplicationRequest file)
        {
            await _licenseApplicationService.SubmitLicenseApplication(file);
            return Ok();
        }


        [HttpGet("ByStaff")]
        [EnableQuery]
        public async Task<ActionResult> GetByStaff()
        {
            var data = await _licenseApplicationService.GetAll();
            if(data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }


        [HttpGet("ByCustomer")]
        [EnableQuery]
        public async Task<ActionResult> GetByCustomer()
        {
            var data = await _licenseApplicationService.GetByCustomer();
            if (data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }


        [HttpPost("Check")]
        public async Task<ActionResult> Update(int licenseApplication,[FromBody]string status)
        {
            if (!Regex.IsMatch(status, @"^(Accepted|Denied)$"))
            {
                return BadRequest(new
                {
                    message = "Status can be Accepted or Denied"
                });
            }
          bool check =   await _licenseApplicationService.UpdateLicenseApplicationByStaff(licenseApplication, status);
            if (!check)
            {
                return BadRequest(new
                {
                    message = "Update UnSucessfully"
                });
            }
            return Ok(new
            {
                message = "Update Sucessfully"
            });
        }

        [HttpPost("Update")]
        public async Task<ActionResult> UpdateByCustomer(int licenseApplication, SubmitLicenseApplicationRequest request)
        {
        await _licenseApplicationService.UpdateLicenseApplicationByCustomer(licenseApplication, request);
            return Ok(new
            {
                message = "Update Sucessfully"
            });
        }

    }
}
