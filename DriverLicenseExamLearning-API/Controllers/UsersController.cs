using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.AspNetCore.Authorization;
//using DriverLicenseExamLearning_Service.ServiceBase.Services;
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
        [Authorize(Roles = RoleNames.Staff + "," + RoleNames.Mentor + "," + RoleNames.Member)]
        [HttpGet()]
        [EnableQuery]
        //https://localhost:7018/api/user?$select=userId,firstname,lastname,email&$filter=status%20eq%20%27Active%27
        public async Task<IActionResult> GetAllOdata()
        {
            var list = await _userService.GetAllAsync();
            return list != null ? Ok(list) : NotFound();
        }


        [EnableQuery]
        [HttpGet("NormalGetWithFilter")]
        public async Task<ActionResult<List<UserResponse>>> GetCustomer([FromQuery] PagingRequest pagingRequest, [FromQuery] UserRequest customerRequest)
        {
            var result = await _userService.GetCustomers(customerRequest, pagingRequest);
            return result != null ? Ok(result) : NotFound();
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> LoginAsync([FromBody] UserLoginRequest loginRequest)
        {
            #region Validate
            var checkRegex = _userService.CheckRegexEmail(loginRequest.Email);
            if (!checkRegex)
            {
                return BadRequest(new
                {
                    msg = "The email is not formatted!"
                });
            }

            var user = await _userService.GetCustomerByEmail(loginRequest.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    msg = "This email is not registered!"
                });
            }
            var checkPassword = _userService.CheckPassword(user, loginRequest.Password);
            if (!checkPassword)
            {
                return BadRequest(new
                {
                    msg = "Wrong Password!"
                });
            }
            #endregion
            var rs = await _userService.LoginAsync(loginRequest);
            return rs != null ? Ok(rs) : BadRequest(new
            {
                msg = "Login Fail!"
            });
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest model)
        {
            #region Validate
            var checkPhoneFormat = _userService.CheckRegexEmail(model.Email);
            if (checkPhoneFormat == false)
            {
                return BadRequest(new
                {
                    message = "Wrong Format Email"
                });
            }
            var checkIsUniquePhone = _userService.IsUniqueUser(model.Email);
            if (checkIsUniquePhone == false)
            {
                return BadRequest(new
                {
                    message = "This email has been registered!"
                });
            }
            #endregion
            var rs = await _userService.RegisterAsync(model);
            return rs != null ? Ok(rs) : BadRequest(new
            {
                msg = "Register Error"
            });
        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken(string refreshToken)
        {
            #region Validate
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new
                {
                    msg = "Refresh Token cannot white space or empty!"
                });
            }
            #endregion
            var refreshTokenResponse = await _userService.RefreshTokenAsync(refreshToken);
            return refreshTokenResponse != null ? Ok(refreshTokenResponse) : BadRequest(new
            {
                msg = "Refresh Token Error!"
            });
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int userId, UserRequest user)
        {
            var rs = await _userService.UpdateAsync(userId, user);
            return rs != null ? Ok(new
            {
                msg = "The profile has been updated!"
            }) : BadRequest(new
            {
                msg = "Update profile error!"
            });
        }


        [HttpPost("mentor-regiser")]
        public async Task<ActionResult> MentorRegister([FromForm] MentorRegisterRequest mentorRegister)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            await _userService.RegisterMentorApplication(mentorRegister);
            return Ok(new
            {
                message = "Register Mentor Sucessfully"
            });
        }

        [HttpGet("IsStaff")]
        [Authorize(Roles = RoleNames.Adminastor)]
        public ActionResult IsStaff()
        {
            return Ok(new
            {
                message = "Is Staff!"
            });
        }

        [HttpDelete]
        public async Task<ActionResult> BanAccount([FromBody]BanAccountRequest request)
        {
           await _userService.BanAccount(request);
            return Ok(new
            {
                message = "Ban This User Successfully"
            }) ; 
        }
    }
}
