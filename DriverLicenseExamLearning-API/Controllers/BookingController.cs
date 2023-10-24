using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/booking")]
    public class BookingController : ODataController
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [SwaggerOperation(Summary = $"[Role:{RoleNames.Staff}][Description:Add New Booking to System]")]
        [Authorize(Roles = RoleNames.Staff)]
        [HttpPost]
        public async Task<ActionResult> CreateBooking([FromBody] BookingRequest req)
        {
            var rs = await _bookingService.CreateBooking(req);
            return rs != null ? Ok(new
            {
                msg = "Create Booking Successfully!"
            }) : BadRequest(new
            {
                msg = "Create Booking Fail!"
            });
        }

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = $"[AllRole][Description:Get All Booking]")]
        [EnableQuery]
        public async Task<ActionResult<BookingResponse>> GetAllAsync()
        {
            var rs = await _bookingService.GetAllBooking();
            return rs != null ? Ok(rs) : NotFound();
        }

        [Authorize(Roles =  RoleNames.Staff)]
        [SwaggerOperation(Summary = $"[Role:{RoleNames.Staff}][Description:Update Booking ]")]
        [HttpPut("{bookingId:int}")]
        public async Task<ActionResult<BookingResponse>> UpdateBooking(int bookingId, [FromBody] BookingRequest req)
        {
            var rs = await _bookingService.UpdateBooking(bookingId, req);
            return rs != null ? Ok(new
            {
                msg = "Update Successfully!"
            }) : BadRequest(new
            {
                msg = "Update fail!"
            });
        }
    }
}
