using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class UserLoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
