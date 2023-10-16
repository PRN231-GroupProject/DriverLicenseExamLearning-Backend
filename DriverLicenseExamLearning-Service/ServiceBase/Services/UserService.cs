//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using DriverLicenseExamLearning_Data.Entity;
//using DriverLicenseExamLearning_Data.UnitOfWork;
//using DriverLicenseExamLearning_Service.DTOs.Request;
//using DriverLicenseExamLearning_Service.DTOs.Response;
//using DriverLicenseExamLearning_Service.ServiceBase.IServices;
//using DriverLicenseExamLearning_Service.Support.Helpers;
//using DriverLicenseExamLearning_Service.Support.Ultilities;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace DriverLicenseExamLearning_Service.ServiceBase.Services
//{

//    public class UserService : IUserService
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly IConfiguration _config;
//        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _config = config;
//        }

//        public bool CheckRegexEmail(string email)
//        {
//            throw new NotImplementedException();
//        }
//        public async Task<IEnumerable<User>> GetAllAsync() => await _unitOfWork.Repository<User>().GetAllAsync();

//        public Task<PagedResult<UserResponse>> GetCustomers(UserRequest request, PagingRequest paging)
//        {
//            var filter = _mapper.Map<UserResponse>(request);
//            var customer = _unitOfWork.Repository<User>()
//                                      .GetAll().Where(x => x.RoleId != 1)
//                                      .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
//                                      .DynamicFilter(filter)
//                                      .ToList();
//            var sort = PageHelper<UserResponse>.Sorting(paging.SortType, customer, paging.ColName);
//            var result = PageHelper<UserResponse>.Paging(sort, paging.Page, paging.PageSize);
//            return Task.FromResult(result);
//        }

//        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
//        {
//            var user = await _unitOfWork.Repository<User>().Where(u => u.Email == request.Email && u.Password == request.Password).FirstOrDefaultAsync();
//            string secretKeyConfig = _config["JWTSecretKey:SecretKey"];

//            DateTime secretKeyDate = DateTime.UtcNow;

//            string refreshToken = RefreshTokenString.GetRefreshToken();
//            user.RefreshToken = refreshToken;

//            await UpdateRefreshToken(user);

//            string accessToken = GenerateJWT(user, secretKeyConfig, secretKeyDate);

//            var RoleResponse = new RoleResponse()
//            {
//                RoleName = user.Role.RoleName
//            };
//            return CreateLoginResponse(user.UserId, user.FirstName, user.LastName, RoleResponse, accessToken, refreshToken);
//        }
//        private async Task UpdateRefreshToken(User user)
//        {
//            await _unitOfWork.Repository<User>().Update(user, user.UserId);
//            await _unitOfWork.CommitAsync();
//        }
//        private string GenerateJWT(User user, string secretKey, DateTime now)
//        {
//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Email, user.Email),
//                new Claim("userId", user.UserId.ToString()),
//                new Claim("role", user.Role.RoleName.ToString())
//            };

//            var token = new JwtSecurityToken(
//                issuer: secretKey,
//                audience: secretKey,
//                claims: claims,
//                expires: now.AddMinutes(86400),
//                signingCredentials: credentials);

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//        private UserLoginResponse CreateLoginResponse(int userId, string firstName, string lastName, RoleResponse role, string accessToken, string refreshToken)
//        {
//            return new UserLoginResponse
//            {
//                UserId = userId,
//                FirstName = firstName,
//                LastName = lastName,
//                Role = role,
//                AccessToken = accessToken,
//                RefreshToken = refreshToken,
//            };
//        }

//        public Task<bool> DeleteUser(int userID)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> UpdateUser(int userID, UserLoginRequest user)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
