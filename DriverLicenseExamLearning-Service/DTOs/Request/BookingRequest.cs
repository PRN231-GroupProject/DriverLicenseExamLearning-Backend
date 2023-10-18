using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class BookingRequest
    {
        public int? MemberId { get; set; }
        public int? PackageId { get; set; }
        public int? MentorId { get; set; }
        public int? CarId { get; set; }
        [JsonIgnore]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}
