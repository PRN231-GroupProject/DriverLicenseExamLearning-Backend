using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class ExamResultDetail
    {
        public int ExamResultDetailsId { get; set; }
        public int? ExamResultId { get; set; }
        public int? QuestionId { get; set; }
        public string? WrongAnswer { get; set; }

        public virtual ExamResult? ExamResult { get; set; }
        public virtual Question? Question { get; set; }
    }
}
