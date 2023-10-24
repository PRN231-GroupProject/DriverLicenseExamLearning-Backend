using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/Tracking")]
    public class TrackingController : ODataController
    {
        private readonly ITrackingService _trackingService;
        public TrackingController(ITrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<TrackingResponse>> GetAll()
        {
            var rs = await _trackingService.GetAllAsync();
            return rs != null ? Ok(rs) : NotFound();
        }

        [Authorize(Roles = RoleNames.Mentor)]
        [HttpPost]
        public async Task<ActionResult<TrackingResponse>> CreateTracking(int bookingId, [FromBody] TrackingRequest req)
        {
            var rs = await _trackingService.CreateTransactionByBookingId(bookingId, req);
            return rs == true ? Ok(new
            {
                msg = "Create new tracking successfully!"
            }) : BadRequest(new
            {
                msg = "Create Fail! - Exceed the number of tracking!"
            });
        }
    }
}
