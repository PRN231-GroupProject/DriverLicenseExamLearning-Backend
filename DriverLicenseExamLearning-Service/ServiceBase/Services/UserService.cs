using AutoMapper;
using AutoMapper.QueryableExtensions;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Helpers;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{

    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        public bool CheckRegexEmail(string email)
        {
            string regex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex re = new Regex(regex);
            if (re.IsMatch(email))
            {
                return true;
            }
            else
                return false;
        }
        public async Task<IEnumerable<User>> GetAllAsync() => await _unitOfWork.Repository<User>().GetAllAsync();

        public Task<PagedResult<UserResponse>> GetCustomers(UserRequest request, PagingRequest paging)
        {
            var filter = _mapper.Map<UserResponse>(request);
            var customer = _unitOfWork.Repository<User>()
                                      .GetAll().Where(x => x.RoleId != 1) //avoid admin
                                      .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                      .DynamicFilter(filter)
                                      .ToList();
            var sort = PageHelper<UserResponse>.Sorting(paging.SortType, customer, paging.ColName);
            var result = PageHelper<UserResponse>.Paging(sort, paging.Page, paging.PageSize);
            return Task.FromResult(result);
        }

        public async Task<UserLoginResponse> LoginAsync(UserLoginRequest request)
        {
            var user =  _unitOfWork.Repository<User>().Include(u => u.Role).Where(u => u.Email == request.Email && u.Password == request.Password).FirstOrDefault();
            string secretKeyConfig = _config["JWTSecretKey:SecretKey"];

            DateTime secretKeyDate = DateTime.UtcNow;

            string refreshToken = RefreshTokenString.GetRefreshToken();
            user.RefreshToken = refreshToken;

            string accessToken = GenerateJWT(user, secretKeyConfig, secretKeyDate);
            user.AccessToken = accessToken;

            await UpdateRefreshToken(user);

            var RoleResponse = new RoleResponse()
            {
                RoleName = user.Role.RoleName
            };
            return CreateLoginResponse(user.UserId, user.Name, user.Email, RoleResponse, accessToken, refreshToken);
        }
        private async Task UpdateRefreshToken(User user)
        {
            await _unitOfWork.Repository<User>().Update(user, user.UserId);
            await _unitOfWork.CommitAsync();
        }
        private string GenerateJWT(User user, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("userId", user.UserId.ToString()),
                new Claim("role", user.Role.RoleName.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: secretKey,
                audience: secretKey,
                claims: claims,
                expires: now.AddMinutes(86400),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserLoginResponse CreateLoginResponse(int userId, string name, string email, RoleResponse role, string accessToken, string refreshToken)
        {
            return new UserLoginResponse
            {
                UserId = userId,
                Name = name,
                Email = email,
                Role = role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        //public async Task<bool> DeleteUser(int userID)
        //{
        //    var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserId == userID);
        //    _unitOfWork.Repository<User>().Delete(user);
        //    await _unitOfWork.CommitAsync();
        //    return _mapper.Map<User, UserResponse>(user);
        //}

        public bool IsUniqueUser(string email)
        {
            var user = _unitOfWork.Repository<User>().Find(x => x.Email == email);
            if (user == null)
            {
                return true;
            } return false;
        }

        public bool CheckPassword(User user, string password)
        {
            return user != null && user.Password == password;
        }

        public async Task<User> GetCustomerByEmail(string email)
        {
            return await _unitOfWork.Repository<User>().FindAsync(x => x.Email == email);
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            
            var newUser = new User
            {
                UserName = request.UserName,
                Password = request.Password,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };
            await _unitOfWork.Repository<User>().CreateAsync(newUser);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserResponse>(newUser);
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Repository<User>()
                .Where(u => u.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();

            string newRefreshToken = RefreshTokenString.GetRefreshToken();
            string secretKeyConfig = _config["JWTSecretKey:SecretKey"];
            DateTime secretKeyDateTime = DateTime.UtcNow;
            string newAccessToken = GenerateJWT(user, secretKeyConfig, secretKeyDateTime);

            user.RefreshToken = newRefreshToken;
            //await UpdateUserWithRefreshToken(user);

            return new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<UserResponse> UpdateAsync(int id, UserRequest request)
        {
            User user = _unitOfWork.Repository<User>().Find(u => u.UserId == id);
            _mapper.Map<UserRequest, User>(request, user);
            await _unitOfWork.Repository<User>().Update(user, id);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<User, UserResponse>(user);
        }
    }
}
