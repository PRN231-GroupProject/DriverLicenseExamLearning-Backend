using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class LicenseType
    {
        public LicenseType()
        {
            Exams = new HashSet<Exam>();
            LicenseApplications = new HashSet<LicenseApplication>();
            MemberAttributes = new HashSet<MemberAttribute>();
            MentorAvailabilities = new HashSet<MentorAvailability>();
            Questions = new HashSet<Question>();
        }

        public int LicenseTypeId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<LicenseApplication> LicenseApplications { get; set; }
        public virtual ICollection<MemberAttribute> MemberAttributes { get; set; }
        public virtual ICollection<MentorAvailability> MentorAvailabilities { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
