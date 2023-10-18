using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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


        [HttpPost]
        public async Task<ActionResult> Submit(SubmitLicenseApplicationRequest file)
        {
            return Ok();
        }
        
    }
}
