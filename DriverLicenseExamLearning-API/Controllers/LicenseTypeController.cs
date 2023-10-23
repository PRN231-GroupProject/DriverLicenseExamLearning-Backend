using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/license-type")]
    public class LicenseTypeController : ODataController
    {
        private readonly ILicenseTypeService _licenseTypeService;
        public LicenseTypeController(ILicenseTypeService licenseTypeService)
        {
            _licenseTypeService = licenseTypeService;
        }
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<LicenseTypeResponse>> GetAllAsync()
        {
            var rs = await _licenseTypeService.GetAllAsync();
            return rs != null ? Ok(rs) : NotFound();
        }



    }
}
