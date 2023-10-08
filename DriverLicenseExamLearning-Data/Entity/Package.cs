using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Package
    {
        public Package()
        {
            Purchases = new HashSet<Purchase>();
        }
        [Key]
        public int PackageId { get; set; }
        public string? Name { get; set; }
        public int? Km { get; set; }
        public int? NumDays { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
