using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class MentorApplicationRequest
    {
        [Required]
        [RegularExpression(@"^(Accepted|Denied)$")]
        public string? Status { get; set; }
        [Required]
        public string? Email { get; set; }


        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
