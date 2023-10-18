using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public  struct LicenseApplicationResponse
    {
        public int userId { get ; set; }


    }


    public struct LicenseApplicationDetailResponse
    {
        public string? LicenseTypeID { get; set; }

        public string? CitizenIdentificationCard { get; set; }

        public string? HealthCertification { get; set; }

        public string? UserImage { get; set; }

        public string? CurriculumVitae { get; set; }
    }
}
