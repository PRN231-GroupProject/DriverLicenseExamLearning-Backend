using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class MemberDayRegisterResponse
    {
        public int? BookingId { get; set; }
        public ICollection<DateTime>? Dates { get; set; }
    }
}
