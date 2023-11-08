using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_gRPC;
using DriverLicenseExamLearning_gRPC.Services;
using DriverLicenseExamLearning_Service.DTOs.Request;


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
