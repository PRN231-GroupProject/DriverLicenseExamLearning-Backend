using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Question
    {
        public int QuestionId { get; set; }
        public string? Text { get; set; }
        public string? Options { get; set; }
        public string? Answer { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
    }
}
