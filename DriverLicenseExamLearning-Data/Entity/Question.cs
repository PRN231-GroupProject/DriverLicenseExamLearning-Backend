using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string? Text { get; set; }
        public string? Options1 { get; set; }
        public string? Options2 { get; set; }
        public string? Options3 { get; set; }
        public string? Options4 { get; set; }
        public string? Answer { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
    }
}
