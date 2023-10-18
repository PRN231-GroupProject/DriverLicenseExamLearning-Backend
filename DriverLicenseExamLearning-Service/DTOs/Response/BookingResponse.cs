using DriverLicenseExamLearning_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class BookingResponse
    {
        public int BookingId { get; set; }
        public ICollection<CarResponse>? Car { get; set; } = new List<CarResponse>();
        public ICollection<UserResponse>? Member { get; set; } = new List<UserResponse>();
        public ICollection<UserResponse>? Mentor { get; set; } = new List<UserResponse>();
        public ICollection<PackageResponse>? Package { get; set; } = new List<PackageResponse>();
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }
    }
}
