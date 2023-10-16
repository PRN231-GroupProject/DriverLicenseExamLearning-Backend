using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? BookingId { get; set; }
        public int? UserId { get; set; }
        public string? Total { get; set; }
        public string? Status { get; set; }

        public virtual Booking? Booking { get; set; }
        public virtual User? User { get; set; }
    }
}
