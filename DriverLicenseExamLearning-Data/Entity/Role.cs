using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }
        [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
