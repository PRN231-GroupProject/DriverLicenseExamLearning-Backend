using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class MemberDayRegister
    {
        public int MemberDayRegisterId { get; set; }
        public int? BookingId { get; set; }
        public DateTime? Datetime { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
