using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class ExamQuestion
    {
        public int ExamQuestions { get; set; }
        public int? ExamId { get; set; }
        public int? QuestionId { get; set; }
        public string? Status { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual Question? Question { get; set; }
    }
}
