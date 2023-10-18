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
        public List<Car>? Car { get; set; } = new List<Car>();
        public List<User>? Member { get; set; } = new List<User>();
        public List<User>? Mentor { get; set; } = new List<User>();
        public List<Package>? Package { get; set; } = new List<Package>();
        public DateTime? CreateDate { get; set; }
        public string? Status { get; set; }
    }
}
