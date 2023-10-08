using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Purchase
    {
        public int PurchaseId { get; set; }
        public int? MemberId { get; set; }
        public int? PackageId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? Status { get; set; }

        public virtual MemberAttribute? Member { get; set; }
        public virtual Package? Package { get; set; }
    }
}
