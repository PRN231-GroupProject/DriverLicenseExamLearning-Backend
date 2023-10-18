using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Components;
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
    }
}
