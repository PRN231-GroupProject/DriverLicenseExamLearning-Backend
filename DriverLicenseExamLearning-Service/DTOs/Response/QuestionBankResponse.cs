using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class QuestionBankResponse
    {
        public int LicenseTypeID { get; set; }

        public string? Name { get ; set; }

        public List<AddQuestionResponse>? questions { get; set; } = new List<AddQuestionResponse>();
    }

    public class AddQuestionResponse
    {
        public int questionId { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public string? Options1 { get; set; }
        [Required]
        public string? Options2 { get; set; }
        [Required]
        public string? Options3 { get; set; }
        [AllowNull]
        public string? Image { get; set; }
        [AllowNull]
        public string? Options4 { get; set; }
        [Required]
        public string? Answer { get; set; }
        [Required]
        public int? LicenseTypeId { get; set; }

        [Required]
        public bool? ParalysisQuestion { get; set; }
    }
}
