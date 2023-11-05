using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/mentor-application")]
    public class MentorApplicationController : ODataController
    {
        private readonly IMentorApplication _application;

        public MentorApplicationController(IMentorApplication application)
        {
            _application = application; 
        }
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<MentorApplicationResponse>>> Get()
        {
            IEnumerable<MentorApplicationResponse> result =await _application.GetMentorApplications();
            if(result == null)
            {
                return NotFound(new
                {
                    message = "Not Found"
                });
            }
            return Ok(result);
        } 
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]MentorApplicationRequest request)
        {
          await  _application.UpdateMentorApplication( request);

            return Ok(new
            {
                message = "Successfully"
            }) ;
        }

    }
}
