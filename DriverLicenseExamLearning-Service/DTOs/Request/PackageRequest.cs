using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class PackageRequest
    {
        public string? PackageName { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }
        public int? PackageTypeId { get; set; }
        public int? NumberOfKmOrDays { get; set; }
        public int? LicenseTypeId { get; set; }
        [JsonIgnore]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}
