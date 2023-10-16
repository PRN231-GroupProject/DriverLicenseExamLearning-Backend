using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Exam
    {
        public Exam()
        {
            ExamResults = new HashSet<ExamResult>();
            Questions = new HashSet<Question>();
        }

        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? LicenseId { get; set; }
        public string? Status { get; set; }

        public virtual LicenseType? License { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
