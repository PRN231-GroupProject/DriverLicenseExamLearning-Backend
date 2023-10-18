using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class PackageType
    {
        public PackageType()
        {
            Packages = new HashSet<Package>();
        }

        public int PackageTypeId { get; set; }
        public string? PackageTypeName { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Package> Packages { get; set; }
    }
}
