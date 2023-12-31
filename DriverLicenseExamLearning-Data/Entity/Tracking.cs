﻿using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Tracking
    {
        public int TrackingId { get; set; }
        public int? BookingId { get; set; }
        public DateTime? TrackingDate { get; set; }
        public string? Note { get; set; }
        public int? Processing { get; set; }
        public string? Status { get; set; }
        public int? Total { get; set; }
        public string? Type { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
