using DriverLicenseExamLearning_Service.DTOs.Request;
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
        Task<IEnumerable<LicenseApplicationDetailResponse>> GetByCustomer();
        Task<IEnumerable<LicenseApplicationResponse>> GetAll();
        Task<bool> SubmitLicenseApplication(int licenseTypeId ,SubmitLicenseApplicationRequest submit);
        Task<bool> UpdateLicenseApplicationByCustomer(int licenseApplicationID,SubmitLicenseApplicationRequest submit);
        public Task<bool> UpdateLicenseApplicationByStaff(UpdateApplicationRequest request);
    }
}
