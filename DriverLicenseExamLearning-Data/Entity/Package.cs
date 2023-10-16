﻿using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Package
    {
        public Package()
        {
            Bookings = new HashSet<Booking>();
        }

        public int PackageId { get; set; }
        public string? PackageName { get; set; }
        public string? Price { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
