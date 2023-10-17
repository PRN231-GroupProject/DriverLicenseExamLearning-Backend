using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class PackageService : IPackagesService
    {
        public Task<PackageResponse> CreatePackage(PackageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PackageResponse> DeletePackage(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PackageResponse> GetPackage(PackageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PackageResponse> GetPackageById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PackageResponse> UpdatePackage(int id, PackageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
