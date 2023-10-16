using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class ExamResult
    {
        public ExamResult()
        {
            ExamResultDetails = new HashSet<ExamResultDetail>();
        }

        public int ExamResultId { get; set; }
        public int? ExamId { get; set; }
        public int? UserId { get; set; }
        public int? AttemptNumber { get; set; }
        public string? Result { get; set; }
        public DateTime? Date { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<ExamResultDetail> ExamResultDetails { get; set; }
    }
}
