using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int? MemberId { get; set; }
        public int? MentorId { get; set; }
        public int? AvailabilityId { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? FeePaid { get; set; }

        public virtual MentorAvailability? Availability { get; set; }
        public virtual MemberAttribute? Member { get; set; }
        public virtual MentorAttribute? Mentor { get; set; }
    }
}
