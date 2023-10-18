using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class TransactionRequest
    {
        public int? BookingId { get; set; }
        public int? UserId { get; set; }
        public string? Total { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}
