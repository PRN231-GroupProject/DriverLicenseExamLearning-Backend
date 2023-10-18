using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class LicenseApplicationService : ILicenseApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;

        public LicenseApplicationService(IUnitOfWork unitOfWork,IClaimsService claimsService)
        {
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
        }
        public async Task<IQueryable<LicenseApplicationResponse>> GetAll()
        {
            var getData = await QueryFormat.GetLicenseApplicationByStaff();
            return getData;

        }

        public Task<IQueryable<LicenseApplicationDetailResponse>> GetByCustomer()
        {
            uint userId = (uint)_claimsService.GetCurrentUserId;
            var getData = QueryFormat.GetLicenseApplicationByCustomer(userId);
            return getData;
        }

        public Task<bool> SubmitLicenseApplication(List<IFormFile> formFiles)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLicenseApplicationByCustomer(int licenseApplicationID, List<IFormFile> formFiles)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateLicenseApplicationByStaff(int licenseApplicationID, string? Status = null)
        {
            throw new NotImplementedException();
        }
    }
}
