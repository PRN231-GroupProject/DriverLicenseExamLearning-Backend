using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Booking
    {
        public Booking()
        {
            MemberDayRegisters = new HashSet<MemberDayRegister>();
            Trackings = new HashSet<Tracking>();
            Transactions = new HashSet<Transaction>();
        }

        public int BookingId { get; set; }
        public int? MemberId { get; set; }
        public int? PackageId { get; set; }
        public int? MentorId { get; set; }
        public int? CarId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }

        public virtual Car? Car { get; set; }
        public virtual User? Member { get; set; }
        public virtual User? Mentor { get; set; }
        public virtual Package? Package { get; set; }
        public virtual ICollection<MemberDayRegister> MemberDayRegisters { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
