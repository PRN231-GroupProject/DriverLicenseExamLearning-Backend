﻿using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class LicenseType
    {
        public LicenseType()
        {
            Exams = new HashSet<Exam>();
            LicenseApplications = new HashSet<LicenseApplication>();
        }

        public int LicenseTypeId { get; set; }
        public string? LicenseName { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<LicenseApplication> LicenseApplications { get; set; }
    }
}
