using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class RegisterRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public int? RoleId { get; set; } = 4;
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}
