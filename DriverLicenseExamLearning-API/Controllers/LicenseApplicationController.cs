using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Swashbuckle.AspNetCore.Annotations;
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


        [Authorize(Roles = RoleNames.Member)]
        [HttpPost("Submit")]
        public async Task<ActionResult> Submit(int licenseTypeID,[FromForm]SubmitLicenseApplicationRequest file)
        {
            await _licenseApplicationService.SubmitLicenseApplication(licenseTypeID,file);
            return Ok(new
            {
                message ="Submit Successfully"
            });
        }

        [Authorize(Roles = RoleNames.Staff)]
        [HttpGet("ByStaff")]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<LicenseApplicationResponse>>> GetByStaff()
        {
            var data = await _licenseApplicationService.GetAll();
            if(data is null )
            {
                return NotFound(new
                {
                    message ="Not Found"
                });
            }
            return Ok(data);
        }

        [Authorize(Roles = RoleNames.Member)]
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

        [Authorize(Roles = RoleNames.Staff)]
        [HttpPost("Check")]
        public async Task<ActionResult> Update([FromBody]UpdateApplicationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
          bool check =   await _licenseApplicationService.UpdateLicenseApplicationByStaff( request);
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

        [Authorize(Roles = RoleNames.Member)]
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
