using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IPackagesService
    {
        Task<IEnumerable<PackageResponse>> GetAllAsync();
        Task<PackageResponse> DeletePackage(int id);
        Task<PackageResponse> CreatePackage(PackageRequest request);
        Task<PackageResponse> UpdatePackage(int id, PackageRequest request);
    }
}
