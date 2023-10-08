using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Exam
    {
        public int ExamId { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? LicenseTypeId { get; set; }
        public string? Status { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
    }
}
