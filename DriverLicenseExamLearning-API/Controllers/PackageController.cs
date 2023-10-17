using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/package")]
    public class PackageController : ODataController
    {
        private readonly IPackagesService _packagesService;
        public PackageController(IPackagesService packagesService)
        {
            _packagesService = packagesService;
        }
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<PackageResponse>> GetAllAsync()
        {
            var rs = await _packagesService.GetAllAsync();
            return rs != null ? Ok(rs) : NotFound();
        }
        [HttpPost]
        public async Task<ActionResult<PackageResponse>> CreatePackage([FromBody] PackageRequest req)
        {
            var rs = await _packagesService.CreatePackage(req);
            return rs != null ? Ok(new
            {
                msg = "Create Package Successfully!"
            }) : BadRequest(new
            {
                msg = "Create fail!"
            });
        }
        [HttpPut("{packageId:int}")]
        public async Task<ActionResult<PackageResponse>> UpdatePackage(int packageId, [FromBody] PackageRequest req)
        {
            var rs = await _packagesService.UpdatePackage(packageId, req);
            return rs != null ? Ok(new
            {
                msg = "Update Successfully!"
            }) : BadRequest(new
            {
                msg = "Update Fail!"
            });
        }
    }
}
