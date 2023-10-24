using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class TrackingResponse
    {
        public int? BookingId { get; set; }
        public DateTime? TrackingDate { get; set; }
        public string? Note { get; set; }
        public int? Processing { get; set; }
        public string? Status { get; set; }
        public int? Total { get; set; }
        public string? Type { get; set; }
    }
}
