using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class LicenseApplication
    {
        public int ApplicationId { get; set; }
        public int? MemberId { get; set; }
        public int? LicenseTypeId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? Status { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
        public virtual MemberAttribute? Member { get; set; }
    }
}
