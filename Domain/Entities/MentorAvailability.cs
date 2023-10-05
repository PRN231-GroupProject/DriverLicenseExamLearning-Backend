﻿using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class MentorAvailability
    {
        public MentorAvailability()
        {
            Bookings = new HashSet<Booking>();
        }

        public int AvailabilityId { get; set; }
        public int? MentorId { get; set; }
        public DateTime? DateTime { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
        public virtual MentorAttribute? Mentor { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
