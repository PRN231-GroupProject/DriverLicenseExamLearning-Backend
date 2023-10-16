using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class LicenseApplication
    {
        public int LicenseApplicationId { get; set; }
        public int? UserId { get; set; }
        public int? LicenseTypeId { get; set; }
        public string? CitizenIdentificationCard { get; set; }
        public string? HealthCertification { get; set; }
        public string? UserImage { get; set; }
        public string? CurriculumVitae { get; set; }
        public string? Status { get; set; }

        public virtual LicenseType? LicenseType { get; set; }
        public virtual User? User { get; set; }
    }
}
