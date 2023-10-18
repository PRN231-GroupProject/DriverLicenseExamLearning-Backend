using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
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
            var p = _mapper.Map<Package>(request);
            await _unitOfWork.Repository<Package>().CreateAsync(p);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<Package, PackageResponse>(p);
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
        public async Task<IEnumerable<Package>> GetAllAsync() => await _unitOfWork.Repository<Package>().GetAllAsync();

        public async Task<PackageResponse> UpdatePackage(int id, PackageRequest request)
        {
            Package package = _unitOfWork.Repository<Package>().Find(u => u.PackageId == id);
            _mapper.Map<PackageRequest, Package>(request, package);
            await _unitOfWork.Repository<Package>().Update(package, id);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<Package, PackageResponse>(package);
        }
    }
}
