using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class MentorAttribute
    {
        public MentorAttribute()
        {
            Bookings = new HashSet<Booking>();
            MentorAvailabilities = new HashSet<MentorAvailability>();
        }
        [Key]
        public int MentorId { get; set; }
        public int? UserId { get; set; }
        public string? Bio { get; set; }
        public string? Experience { get; set; }
        public string? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<MentorAvailability> MentorAvailabilities { get; set; }
    }
}
