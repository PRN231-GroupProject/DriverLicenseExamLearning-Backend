using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IClaimsService
    {
        public int GetCurrentUserId { get; }
        public string GetCurrentUserRole { get; }
    }
}
