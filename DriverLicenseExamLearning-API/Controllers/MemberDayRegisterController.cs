﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/MemberDayRegister")]
    public class MemberDayRegisterController : ODataController
    {
        private readonly IMemberDayRegisterService _memberDayRegisterService;
        public MemberDayRegisterController(IMemberDayRegisterService memberDayRegisterService)
        {
            _memberDayRegisterService = memberDayRegisterService;
        }
        [Authorize(Roles = RoleNames.Member)]
        [HttpPost("{bookingId:int}")]
        public async Task<ActionResult<MemberDayRegisterResponse>> CreateMemberDayRegisterByBookingId(int bookingId, [FromBody] MemberDayRegisterRequest req)
        {
            #region validate before input
            if (req == null)
            {
                return BadRequest(new
                {
                    msg = "Wrong day format or missing type of date!!!"
                });
            }
            #endregion
            var rs = await _memberDayRegisterService.CreateMemberDayRegisterByBookingId(bookingId, req);
            return rs != null ? Ok(new
            {
                msg = "Register Day Successfully!"
            }) : BadRequest(new
            {
                msg = "Register Day Failed!"
            });
        }
        [Authorize(Roles = RoleNames.Mentor + "," + RoleNames.Member)]
        [HttpGet("{bookingId:int}")]
        public async Task<ActionResult<MemberDayRegisterRequest>> GetMemberDayRegisterByBookingId(int bookingId)
        {
            var rs = await _memberDayRegisterService.GetMemberDayRegisterByBookId(bookingId);
            return rs != null ? Ok(rs) : NotFound();
        }
    }
}
