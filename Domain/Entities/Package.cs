using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Package
    {
        public Package()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int PackageId { get; set; }
        public string? Name { get; set; }
        public int? Km { get; set; }
        public int? NumDays { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
