using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class PackageTypeResponse
    {
        //public int PackageTypeId { get; set; }
        public string? PackageTypeName { get; set; }
        [JsonIgnore]
        public string? Status { get; set; }
    }
}
