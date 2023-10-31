using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class MemberDayRegisterRequest
    {
        public ICollection<DateTime>? Dates { get; set; }
    }
}
