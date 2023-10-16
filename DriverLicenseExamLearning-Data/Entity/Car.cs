using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Car
    {
        public Car()
        {
            Bookings = new HashSet<Booking>();
        }

        public int CarId { get; set; }
        public string? CarName { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public string? CarType { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
