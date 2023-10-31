using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class MentorApplicationRequest
    {
        public string? Status { get; set; }

        public string? Email { get; set; }


        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
