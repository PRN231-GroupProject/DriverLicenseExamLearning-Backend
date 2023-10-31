using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class UpdateCarRequest
    {
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string? CarName { get; set; }

      
        public string? Image { get ; set; }

        [Required]
        [RegularExpression(@"^(Active|InActive)$")]
        public string? Status { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string? CarType { get; set; }
    }
}
