using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class TrackingRequest
    {
        public int? BookingId { get; set; }
        [JsonIgnore]
        public DateTime? TrackingDate { get; set; } = DateTime.Now;
        public string? Note { get; set; }
        public string? Processing { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}
