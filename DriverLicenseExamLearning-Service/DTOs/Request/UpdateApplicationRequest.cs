using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public  class UpdateApplicationRequest
    {
        [Required]
        [RegularExpression(@"^(Accepted|Denied)$")]
        public string? Status { get; set; }
        [Required]
        public string? Message { get; set; }
    }
}
