using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
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

        public LicenseApplicationService(IUnitOfWork unitOfWork, IClaimsService claimsService)
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



        public async Task<bool> SubmitLicenseApplication(SubmitLicenseApplicationRequest submit)
        {

           FireBaseFile fileCitizenCard =  await FirebaseHelper.UploadFileAsync(submit.CitizenIdentificationCard, "license-application");
         FireBaseFile fileHealthCer  = await FirebaseHelper.UploadFileAsync(submit.HealthCertification, "license-application");
         FireBaseFile fileCV =    await FirebaseHelper.UploadFileAsync(submit.CurriculumVitae, "license-application");
     FireBaseFile fileImage    =   await FirebaseHelper.UploadFileAsync(submit.UserImage, "license-application");
            LicenseApplication licenseApplication = new LicenseApplication()
            {
                CitizenIdentificationCard = fileCitizenCard.URL,
                CurriculumVitae = fileCV.URL,
                HealthCertification = fileHealthCer.URL,
                UserImage = fileImage.URL,

            };

            await _unitOfWork.Repository<LicenseApplication>().CreateAsync(licenseApplication);
            _unitOfWork.Commit();
            return true;
        }



        public async Task<bool> UpdateLicenseApplicationByCustomer(int licenseApplicationID, SubmitLicenseApplicationRequest submit)
        {
          var licenseApplicationFind =  _unitOfWork.Repository<LicenseApplication>().Where(x => x.LicenseApplicationId == licenseApplicationID).FirstOrDefault();
            if(submit.CurriculumVitae is not null)
            {

              FireBaseFile file =  await FirebaseHelper.UploadFileAsync(submit.CurriculumVitae, "license-application");
                licenseApplicationFind.CurriculumVitae = file.FileName;
            }
            if (submit.CurriculumVitae is not null)
            {

                FireBaseFile file = await FirebaseHelper.UploadFileAsync(submit.CitizenIdentificationCard, "license-application");
                licenseApplicationFind.CitizenIdentificationCard = file.FileName;
            }
            if (submit.CurriculumVitae is not null)
            {

                FireBaseFile file = await FirebaseHelper.UploadFileAsync(submit.HealthCertification, "license-application");
                licenseApplicationFind.HealthCertification = file.FileName;
            }
            if (submit.CurriculumVitae is not null)
            {

                FireBaseFile file = await FirebaseHelper.UploadFileAsync(submit.UserImage, "license-application");
                licenseApplicationFind.UserImage = file.FileName;
            }

           await _unitOfWork.Repository<LicenseApplication>().Update(licenseApplicationFind, licenseApplicationID);
            _unitOfWork.Commit();
            return true;
        }

        public  async Task<bool> UpdateLicenseApplicationByStaff(int licenseApplicationID, string? Status = null)
        {
           var licenseApplication = _unitOfWork.Repository<LicenseApplication>().Where(x => x.LicenseApplicationId == licenseApplicationID).FirstOrDefault();
            if (licenseApplication != null)
            {
                licenseApplication.Status = Status;
                await _unitOfWork.Repository<LicenseApplication>().Update(licenseApplication, licenseApplicationID);
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }
    }
}
