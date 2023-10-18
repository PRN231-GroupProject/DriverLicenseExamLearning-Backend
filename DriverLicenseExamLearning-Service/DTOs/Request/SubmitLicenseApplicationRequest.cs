using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class SubmitLicenseApplicationRequest
    {
        public string? LicenseTypeID { get; set; }

        public string? CitizenIdentificationCard { get; set;}
  
        public string? HealthCertification { get; set;}   

        public string? UserImage  { get; set;}

        public string? CurriculumVitae { get; set;}
    }
}
