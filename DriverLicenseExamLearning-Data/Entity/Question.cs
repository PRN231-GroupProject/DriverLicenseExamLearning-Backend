﻿using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Question
    {
        public Question()
        {
            ExamResultDetails = new HashSet<ExamResultDetail>();
        }

        public int QuestionId { get; set; }
        public int? ExamId { get; set; }
        public string? Question1 { get; set; }
        public string? Image { get; set; }
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Answer { get; set; }
        public string? Status { get; set; }

        public virtual Exam? Exam { get; set; }
        public virtual ICollection<ExamResultDetail> ExamResultDetails { get; set; }
    }
}
