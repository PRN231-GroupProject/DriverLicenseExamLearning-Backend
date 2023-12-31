﻿using DriverLicenseExamLearning_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class UserLoginResponse
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public virtual RoleResponse? Role { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

    }
}
