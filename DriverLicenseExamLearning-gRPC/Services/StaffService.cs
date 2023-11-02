using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Response;
using FirebaseAdmin.Messaging;
using Grpc.Core;
using System;

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
        public override async Task<ReponseModel> CreateNewStaff(NewStaffRequest request, ServerCallContext context)
        {
            var token = context.RequestHeaders.GetValue("authorization");
            var isAdmin = await CheckAdmin(token);
            if (!isAdmin)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Required admin"));
            }


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

            await _unitOfWork.Repository<User>().CreateAsync(newUser);
            await _unitOfWork.CommitAsync();
            var reponse = new ReponseModel()
            {
                Message = "Added"
            };
            return reponse;
        }

        public override async Task<StaffReponse> GetStaff(StaffLookUpModel request, ServerCallContext context)
        {
            var token = context.RequestHeaders.GetValue("authorization");
            var isAdmin = await CheckAdmin(token);
            if (!isAdmin)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Required admin"));
            }

            var user = await _unitOfWork.Repository<User>().GetById(request.UserId);

            if (user != null && user.RoleId == 2)
            {
                // Map the values from 'user' to 'StaffReponse'
                StaffReponse response = new StaffReponse
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Address = user.Address,
                    Email = user.Email,
                    Name = user .UserName,
                    Password= user.Password,
                    Status = user.Status,
                };

                return response;
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
        }

        public override async Task<GetStaffsReponse> GetStaffs(RequestModel request, ServerCallContext context)
        {
            var token = context.RequestHeaders.GetValue("authorization");
            var isAdmin = await CheckAdmin(token);
            if (!isAdmin)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Required admin"));
            }

            var users = await _unitOfWork.Repository<User>().GetWhere(o => o.RoleId == 2);

            if (users != null)
            {
                List<StaffReponse> results = new List<StaffReponse>();
                foreach (var user in users)
                {
                    results.Add(
                        new StaffReponse
                        {
                            UserId = user.UserId,
                            UserName = token,
                            Address = user.Address,
                            Email = user.Email,
                            Name = user.UserName,
                            Password = user.Password,
                            Status = user.Status,
                            PhoneNumber = user.PhoneNumber,
                        }
                        );
                }
                var reponse = new GetStaffsReponse();
                reponse.Staffs.AddRange(results);
                return reponse;
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "There are no staff"));
            }
        }
        public override async Task<ReponseModel> DeleteStaff(StaffLookUpModel request, ServerCallContext context)
        {
            var token = context.RequestHeaders.GetValue("authorization");
            var isAdmin = await CheckAdmin(token);
            if (!isAdmin)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Required admin"));
            }


            var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserId == request.UserId && u.RoleId == 2);
            if(user != null)
            {
                _unitOfWork.Repository<User>().Delete(user);
                await _unitOfWork.CommitAsync();
                return new ReponseModel()
                {
                    Message = "Deleted"
                };
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Staff not found"));
            }


        }

        public override async Task<ReponseModel> UpdateStaff(UpdateStaffRequest request, ServerCallContext context)
        {
            var token = context.RequestHeaders.GetValue("authorization");
            var isAdmin = await CheckAdmin(token);
            if (!isAdmin)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Required admin"));
            }

            var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserId == request.UserId && u.RoleId == 2);
            if (user != null)
            {
                user.UserName = request.UserName;
                user.Password = request.Password;
                user.Email = request.Email;
                user.Name = request.Name;
                user.PhoneNumber = request.PhoneNumber;
                user.Address = request.Address;
                user.Status = request.Status;

                _unitOfWork.Repository<User>().Update(user, user.UserId);
                await _unitOfWork.CommitAsync();
                return new ReponseModel()
                {
                    Message = "Updated"
                };
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Staff not found"));
            }
        }

        public async Task<bool> CheckAdmin(string token)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"{token}");
            var response = await client.GetAsync(ApiConstans.ApiCheckStaff);
            //  var response = await client.GetAsync(ApiConstans.ApiCheckStaffDeploy);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }


}
