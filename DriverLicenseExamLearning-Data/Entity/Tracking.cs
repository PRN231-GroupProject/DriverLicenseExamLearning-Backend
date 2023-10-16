using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Tracking
    {
        public Tracking()
        {
            Bookings = new HashSet<Booking>();
        }

        public int TrackingId { get; set; }
        public DateTime? TrackingDate { get; set; }
        public string? Note { get; set; }
        public string? Processing { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
