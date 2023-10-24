using DriverLicenseExamLearning_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class PackageResponse
    {
        public int? PackageId { get; set; }
        public int? PackageTypeId { get; set; }
        public string? PackageName { get; set; }
        public int? Price { get; set; }
        public int? NumberOfKmOrDays { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
        public ICollection<LicenseTypeResponse>? LicenseType { get; set; } = new List<LicenseTypeResponse>();
        public ICollection<PackageTypeResponse>? PackageTypes { get; set; } = new List<PackageTypeResponse>();
    }
}
