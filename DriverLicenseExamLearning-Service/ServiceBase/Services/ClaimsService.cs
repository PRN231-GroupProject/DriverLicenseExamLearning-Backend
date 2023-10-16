using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class ClaimsService : IClaimsService
    {
        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirstValue("userId");
            GetCurrentUserId = int.TryParse(id, out int userId) ? GetCurrentUserId = userId : GetCurrentUserId = 0;
            var role = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
            GetCurrentUserRole = string.IsNullOrEmpty(role) ? string.Empty : role;
        }
        public int GetCurrentUserId { get; }
        public string GetCurrentUserRole { get; }
    }
}
