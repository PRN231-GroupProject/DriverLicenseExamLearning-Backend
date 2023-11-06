using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.HandleError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class PackageService : IPackagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<PackageResponse> CreatePackage(PackageRequest request)
        {
            if(await CheckPackageName(request.PackageName))
            {
            var p = _mapper.Map<Package>(request);
            await _unitOfWork.Repository<Package>().CreateAsync(p);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<Package, PackageResponse>(p);
            }
            return null;
        }

        public Task<PackageResponse> DeletePackage(int id)
        {
            throw new NotImplementedException();
        }

        //public async Task<PackageResponse> GetAllAsync(PackageRequest request)
        //{
        //    var p = _mapper.Map<Package>(request);
        //    await _unitOfWork.Repository<Package>().GetAllAsync();
        //    await _unitOfWork.CommitAsync();
        //    return _mapper.Map<Package, PackageResponse>(p);
        //}
        public async Task<IEnumerable<PackageResponse>> GetAllAsync()
        {
            var packageResponses = await _unitOfWork.Repository<Package>()
                .Include(p => p.LicenseType)
                .Include(p => p.PackageType) // Adjust the navigation property name
                .Select(p => new PackageResponse
                {
                    PackageId = p.PackageId,
                    PackageName = p.PackageName,
                    PackageTypeId = p.PackageTypeId,
                    NumberOfKmOrDays = p.NumberOfKmOrDays,
                    Price = p.Price,
                    Description = p.Description,
                    CreateDate = p.CreateDate,
                    Status = p.Status,
                    LicenseType = new List<LicenseTypeResponse>
                    {
                        new LicenseTypeResponse
                        {
                            LicenseName = p.LicenseType.LicenseName
                        }
                    },
                    PackageTypes = new List<PackageTypeResponse>
                    {
                        new PackageTypeResponse
                        {
                            PackageTypeName = p.PackageType.PackageTypeName,
                            Status = p.PackageType.Status
                        }
                    }
                })
                .ToListAsync();

            return packageResponses;
        }


        public async Task<PackageResponse> UpdatePackage(int id, PackageRequest request)
        {
            Package package = _unitOfWork.Repository<Package>().Find(u => u.PackageId == id);
            _mapper.Map<PackageRequest, Package>(request, package);
            await _unitOfWork.Repository<Package>().Update(package, id);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<Package, PackageResponse>(package);
        }



        private async Task<bool> CheckPackageName(string PackageName)
        {
            var package = _unitOfWork.Repository<Package>().Where(x => x.PackageName == PackageName);
            if (package is not null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "This Package Name have already used in system");
            }
            return true;
        }
    }
}
