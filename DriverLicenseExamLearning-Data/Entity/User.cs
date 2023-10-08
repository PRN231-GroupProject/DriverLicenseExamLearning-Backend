using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class User
    {
        public User()
        {
            MemberAttributes = new HashSet<MemberAttribute>();
            MentorAttributes = new HashSet<MentorAttribute>();
        }

        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
        public string? RefreshToken { get; set; }
        public string? Status { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<MemberAttribute> MemberAttributes { get; set; }
        public virtual ICollection<MentorAttribute> MentorAttributes { get; set; }
    }
}
