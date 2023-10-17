using DriverLicenseExamLearning_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface ILicenseTypeService
    {
        Task<IEnumerable<LicenseType>> GetAllAsync();
    }
}
