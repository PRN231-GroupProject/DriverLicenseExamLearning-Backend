using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class BanAccountRequest
    {
        public int AccountId { get; set; }

        public string? Reason { get; set; }
    }
}
