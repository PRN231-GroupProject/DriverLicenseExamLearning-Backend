using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class SubmitLicenseApplicationRequest
    {
        public IFormFile? LicenseTypeID { get; set; }

        public IFormFile? CitizenIdentificationCard { get; set;}
  
        public IFormFile? HealthCertification { get; set;}   

        public IFormFile? UserImage  { get; set;}

        public IFormFile? CurriculumVitae { get; set;}
    }
}
