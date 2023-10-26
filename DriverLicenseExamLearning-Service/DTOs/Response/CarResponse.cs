using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class CarResponse
    {
        public int? CarId { get; set; }
        public string? CarName { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public string? CarType { get; set; }
    }
}
