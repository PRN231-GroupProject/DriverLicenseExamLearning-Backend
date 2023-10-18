using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class PackageType
    {
        public int PackageTypeId { get; set; }
        public int? PackageId { get; set; }
        public string? PackageTypeName { get; set; }
        public string? Status { get; set; }

        public virtual Package? Package { get; set; }
    }
}
