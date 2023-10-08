using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class ExamQuestion
    {
        public int ExamQuestionId { get; set; }
        public int? ExamId { get; set; }
        public int? QuestionId { get; set; }
        public string? Status { get; set; }

        public virtual Exam Exam { get; set; } = null!;
        public virtual Question? Question { get; set; }
    }
}
