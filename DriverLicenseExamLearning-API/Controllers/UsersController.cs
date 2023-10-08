using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetCustomer([FromQuery] PagingRequest pagingRequest, [FromQuery] UserRequest customerRequest)
        {
            var result = await _userService.GetCustomers(customerRequest, pagingRequest);
            return result != null ? Ok(result) : NotFound();
        }
    }
}
