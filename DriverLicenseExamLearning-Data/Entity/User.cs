using System;
using System.Collections.Generic;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class User
    {
        public User()
        {
            BookingMembers = new HashSet<Booking>();
            BookingMentors = new HashSet<Booking>();
            ExamResults = new HashSet<ExamResult>();
            LicenseApplications = new HashSet<LicenseApplication>();
            MentorAttributes = new HashSet<MentorAttribute>();
            Transactions = new HashSet<Transaction>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Booking> BookingMembers { get; set; }
        public virtual ICollection<Booking> BookingMentors { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; }
        public virtual ICollection<LicenseApplication> LicenseApplications { get; set; }
        public virtual ICollection<MentorAttribute> MentorAttributes { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
