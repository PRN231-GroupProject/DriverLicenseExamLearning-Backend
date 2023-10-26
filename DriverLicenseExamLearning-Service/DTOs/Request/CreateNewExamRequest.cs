using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public  class CreateNewExamRequest
    {
        [RegularExpression(@"^(Private|Public)$")]
        public string? Status { get; set; }
        [Required]
        [MinLength(1)]
        public string? ExamName { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int? LicenseTypeId { get; set; }
        [Required]
        public List<int>? QuestionID { get; set; }




    }
}
