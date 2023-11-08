using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.HandleError;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<LicenseApplicationResponse>> GetAll()
        {
            //var getData = await QueryFormat.GetLicenseApplicationByStaff();
            var result = await _unitOfWork.Repository<LicenseApplication>().Include(x => x.User).GroupBy(x => x.UserId).Select(group => new LicenseApplicationResponse
            {
                userId = group.First().User.UserId,
                loader = group.Select(x => new LicenseApplicationDetailResponse
                {
                  
                        LicenseTypeID = x.LicenseTypeId,
                        LicenseApplicationId = x.LicenseApplicationId,
                        CitizenIdentificationCard = x.CitizenIdentificationCard,
                        CurriculumVitae = x.CurriculumVitae,
                        HealthCertification = x.HealthCertification,
                        Status = x.Status,
                        UserImage = x.UserImage
                   
                }).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<LicenseApplicationDetailResponse>> GetByCustomer()
        {
            var userId = _claimsService.GetCurrentUserId;

            IEnumerable<LicenseApplicationDetailResponse> result = await _unitOfWork.Repository<LicenseApplication>().Include(x => x.User).Where(x => x.User.UserId == userId).Select(x => new LicenseApplicationDetailResponse
            {
            
                CitizenIdentificationCard = x.CitizenIdentificationCard,
                CurriculumVitae = x.CurriculumVitae,
                HealthCertification = x.HealthCertification,
                LicenseApplicationId = x.LicenseApplicationId,
                LicenseTypeID = x.LicenseTypeId,
                Status = x.Status,
                UserImage = x.UserImage
            }).ToListAsync();
            return result;
        }



        public async Task<bool> SubmitLicenseApplication(int LicenseTypeId, SubmitLicenseApplicationRequest submit)
        {

            var userId = _claimsService.GetCurrentUserId;

          var checkSubmitinLicenseApplication  = _unitOfWork.Repository<LicenseApplication>().Where(x => x.UserId == userId && x.LicenseTypeId == LicenseTypeId).FirstOrDefault();
            if(checkSubmitinLicenseApplication != null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "You have already submit licenseapplication ");
            }
            FireBaseFile fileCitizenCard = await FirebaseHelper.UploadFileAsync(submit.CitizenIdentificationCard, "license-application");
            FireBaseFile fileHealthCer = await FirebaseHelper.UploadFileAsync(submit.HealthCertification, "license-application");
            FireBaseFile fileCV = await FirebaseHelper.UploadFileAsync(submit.CurriculumVitae, "license-application");
            FireBaseFile fileImage = await FirebaseHelper.UploadFileAsync(submit.UserImage, "license-application");

            LicenseApplication licenseApplication = new LicenseApplication()
            {
                CitizenIdentificationCard = fileCitizenCard.URL,
                CurriculumVitae = fileCV.URL,
                HealthCertification = fileHealthCer.URL,
                UserImage = fileImage.URL,
                UserId = userId,
                Status = "Proccessing",
                LicenseTypeId = LicenseTypeId, 

            };

            await _unitOfWork.Repository<LicenseApplication>().CreateAsync(licenseApplication);
            _unitOfWork.Commit();
            return true;
        }



        public async Task<bool> UpdateLicenseApplicationByCustomer(int licenseApplicationID, SubmitLicenseApplicationRequest submit)
        {
            var licenseApplicationFind = _unitOfWork.Repository<LicenseApplication>().Where(x => x.LicenseApplicationId == licenseApplicationID).FirstOrDefault();
            if (submit.CurriculumVitae is not null)
            {

                FireBaseFile file1 = await FirebaseHelper.UploadFileAsync(submit.CurriculumVitae, "license-application");
                licenseApplicationFind.CurriculumVitae = file1.FileName;
            }
            if (submit.CitizenIdentificationCard is not null)
            {

                FireBaseFile file2 = await FirebaseHelper.UploadFileAsync(submit.CitizenIdentificationCard, "license-application");
                licenseApplicationFind.CitizenIdentificationCard = file2.FileName;
            }
            if (submit.HealthCertification is not null)
            {

                FireBaseFile file3 = await FirebaseHelper.UploadFileAsync(submit.HealthCertification, "license-application");
                licenseApplicationFind.HealthCertification = file3.FileName;
            }
            if (submit.UserImage is not null)
            {

                FireBaseFile file4 = await FirebaseHelper.UploadFileAsync(submit.UserImage, "license-application");
                licenseApplicationFind.UserImage = file4.FileName;
            }

            await _unitOfWork.Repository<LicenseApplication>().Update(licenseApplicationFind, licenseApplicationID);
          await  _unitOfWork.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateLicenseApplicationByStaff(UpdateApplicationRequest request)
        {
            LicenseApplication licenseApplication = _unitOfWork.Repository<LicenseApplication>().Where(x => x.LicenseTypeId == request.LicenseTypeID && x.UserId == request.UserID).FirstOrDefault();
            if (licenseApplication != null)
            {
                licenseApplication.Status = request.Status;
                licenseApplication.Message = request.Message;
                await _unitOfWork.Repository<LicenseApplication>().Update(licenseApplication, licenseApplication.LicenseApplicationId);
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }
    }
}
