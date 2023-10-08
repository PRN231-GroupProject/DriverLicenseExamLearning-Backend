using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/user")]
    public class UsersController : ODataController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [EnableQuery]
        //https://localhost:7018/api/user?$select=userId,firstname,lastname,email&$filter=status%20eq%20%27Active%27
        public async Task<IActionResult> GetAllOdata()
        {
            var list = await _userService.GetAllAsync();
            return list != null ? Ok(list) : NotFound();
        }

        [HttpGet("NormalGetWithFilter")]
        public async Task<ActionResult<List<UserResponse>>> GetCustomer([FromQuery] PagingRequest pagingRequest, [FromQuery] UserRequest customerRequest)
        {
            var result = await _userService.GetCustomers(customerRequest, pagingRequest);
            return result != null ? Ok(result) : NotFound();
        }

    }
}
