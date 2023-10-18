using DriverLicenseExamLearning_Service.DTOs.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface ILicenseApplicationService
    {
        Task<IQueryable<LicenseApplicationDetailResponse>> GetByCustomer();
        Task<IQueryable<LicenseApplicationResponse>> GetAll();
        Task<bool> SubmitLicenseApplication(List<IFormFile> formFiles);
        Task<bool> UpdateLicenseApplicationByCustomer(int licenseApplicationID,List<IFormFile> formFiles);
        Task<bool> UpdateLicenseApplicationByStaff(int licenseApplicationID, string? Status = null);
    }
}
