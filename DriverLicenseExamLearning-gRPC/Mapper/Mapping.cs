using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_gRPC;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;

namespace Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            #region User & Role
            CreateMap<User, StaffReponse>().ReverseMap();
            CreateMap<User, UserLoginRequest>().ReverseMap();

            #endregion


        }
    }
}
