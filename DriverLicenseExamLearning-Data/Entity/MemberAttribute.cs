using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class MemberAttribute
    {
        public MemberAttribute()
        {
            Bookings = new HashSet<Booking>();
            LicenseApplications = new HashSet<LicenseApplication>();
            Purchases = new HashSet<Purchase>();
        }
        [Key]
        public int MemberId { get; set; }
        public int? UserId { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<LicenseApplication> LicenseApplications { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
