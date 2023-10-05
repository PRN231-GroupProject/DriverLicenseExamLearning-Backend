using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class ExamQuestion
    {
        public int? ExamId { get; set; }
        public int? QuestionId { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual Question? Question { get; set; }
    }
}
