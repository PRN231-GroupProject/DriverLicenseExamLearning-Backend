using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class RefreshTokenResponse
    {
        public string? AccessToken { get; internal set; }
        public string? RefreshToken { get; internal set; }
    }
}
