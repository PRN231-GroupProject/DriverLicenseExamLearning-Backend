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
        [JsonIgnore]
        public int? BookingId { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public int? Total { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }
        [JsonIgnore]
        public string? TransactionType { get; set; }
    }
}
