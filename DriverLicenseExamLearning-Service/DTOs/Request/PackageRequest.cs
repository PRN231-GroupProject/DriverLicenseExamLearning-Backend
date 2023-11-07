using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class PackageRequest : IValidatableObject
    {
        [Required]
        [MinLength(10)]
        public string? PackageName { get; set; }
        [Required]
        [Range(500000,20000000)]
        public int? Price { get; set; }
        public string? Description { get; set; }
        [Range(1,2)]
        public int? PackageTypeId { get; set; }
        public int? NumberOfKmOrDays { get; set; }
        public int? LicenseTypeId { get; set; }
        [JsonIgnore]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [JsonIgnore]
        public string? Status { get; set; } = "Active";

          public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PackageTypeId == 1)
            {
                if(NumberOfKmOrDays < 1 || NumberOfKmOrDays > 10000)
                {
                    yield return new ValidationResult("With Km Type Range be [1,10000]");
                }

                
            }
            if (PackageTypeId == 2)
            {
                if (NumberOfKmOrDays > 1 || NumberOfKmOrDays < 30)
                {
                    yield return new ValidationResult("With Day Type Range be[1,30]");
                }
            }
        }
          
    }
}
