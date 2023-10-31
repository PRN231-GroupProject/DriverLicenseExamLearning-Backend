using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class TransactionResponse
    {
        public int? BookingId { get; set; }
        public int? UserId { get; set; }
        public int? Total { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }
    }
}
