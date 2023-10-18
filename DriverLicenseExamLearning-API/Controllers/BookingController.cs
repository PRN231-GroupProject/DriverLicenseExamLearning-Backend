using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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
        [HttpPost]
        public async Task<ActionResult> CreateBooking([FromBody] BookingRequest req)
        {
            var rs = await _bookingService.CreateBooking(req);
            return rs != null ? Ok(rs) : BadRequest(new
            {
                msg = "Create Booking Fail!"
            });
        }
    }
}
