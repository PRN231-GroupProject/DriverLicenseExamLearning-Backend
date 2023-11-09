using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class MentorRegisterRequest
    {
        [Required]
        public IFormFile Bio { get ; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Experience { get; set; }
    }
}
