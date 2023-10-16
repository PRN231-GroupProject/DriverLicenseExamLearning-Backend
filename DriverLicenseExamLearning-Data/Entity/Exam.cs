using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Exam
    {
        public Exam()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
            ExamResults = new HashSet<ExamResult>();
        }

        public int ExamId { get; set; }
        public string? ExamName { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? LicenseId { get; set; }
        public string? Status { get; set; }

        public virtual LicenseType? License { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
    }
}
