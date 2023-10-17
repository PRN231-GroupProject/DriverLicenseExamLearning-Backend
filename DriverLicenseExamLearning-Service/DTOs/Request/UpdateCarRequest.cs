using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class UpdateCarRequest
    {
        public string? CarName { get; set; }

        public string? Image { get ; set; }

        public string? Status { get; set; }
       
        public string? CarType { get; set; }
    }
}
