using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;

namespace DriverLicenseExamLearning_API.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<User, UserLoginRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, UserLoginResponse>().ReverseMap();
            CreateMap<User, UserLoginResponse>().ReverseMap();
            CreateMap<UserRequest, UserResponse>().ReverseMap();

            CreateMap<Role, RoleResponse>().ReverseMap();


            CreateMap<CreateNewExamRequest , Exam>()
                .ForMember(x => x.ExamName, opt => opt.MapFrom(x => x.ExamName))
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status))
                .ForMember(x => x.LicenseId, opt => opt.MapFrom(x => x.LicenseTypeId))
                .ReverseMap();
            //Quiz Mapping
            //CreateMap<AddQuestionRequest, Question>()
            //    .ForMember(dex => dex.LicenseTypeId, opt => opt.MapFrom(src => src.LicenseTypeId))
            //    .ForMember(dex => dex.Text, opt => opt.MapFrom(src => src.Text))
            //    .ForMember(dex => dex.Options1, opt => opt.MapFrom(src => src.Options1))
            //    .ForMember(dex => dex.Options2, opt => opt.MapFrom(src => src.Options2))
            //    .ForMember(dex => dex.Options3, opt => opt.MapFrom(src => src.Options3))
            //    .ForMember(dex => dex.Options4, opt => opt.MapFrom(src => src.Options4))
            //    .ForMember(dex => dex.Answer, opt => opt.MapFrom(src => src.Answer))
            //    .ReverseMap();

            CreateMap<UpdateCarRequest, Car>()
                .ForMember(x => x.CarType, opt => opt.MapFrom(x => x.CarType))
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status))
                .ForMember(x => x.CarName, opt => opt.MapFrom(x => x.CarName))
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status))
                .ReverseMap();
        }
    }
}
