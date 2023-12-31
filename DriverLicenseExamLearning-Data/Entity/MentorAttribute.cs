﻿using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class MentorAttribute
    {
        public int MentorAttributeId { get; set; }
        public int? UserId { get; set; }
        public string? Bio { get; set; }
        public string? Experience { get; set; }
        public string? Status { get; set; }

        public virtual User? User { get; set; }
    }
}
