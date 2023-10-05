using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Exam
    {
        public int ExamId { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
    }
}
