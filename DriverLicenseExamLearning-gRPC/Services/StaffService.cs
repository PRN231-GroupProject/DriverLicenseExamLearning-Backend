using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Response;
using Grpc.Core;

namespace DriverLicenseExamLearning_gRPC.Services
{
    public class StaffService :Staff.StaffBase
    {
        private readonly ILogger<StaffService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public StaffService(ILogger<StaffService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public override Task<NewStaffRequest> CreateNewStaff(NewStaffRequest request, ServerCallContext context)
        {
            var newUser = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                RoleId = 2,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Status = "Active",
            };

            Task task1 = _unitOfWork.Repository<User>().CreateAsync(newUser);
            Task task2 = _unitOfWork.CommitAsync();

            return Task.WhenAll(task1, task2).ContinueWith(_ => request);
        }
    }


}
