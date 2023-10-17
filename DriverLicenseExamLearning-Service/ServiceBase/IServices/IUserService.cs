using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IUserService
    {
        Task<PagedResult<UserResponse>> GetCustomers(UserRequest request, PagingRequest paging);
        bool CheckRegexEmail(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<UserLoginResponse> LoginAsync(UserLoginRequest request);
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<User> GetCustomerByEmail(string email);
        bool IsUniqueUser(string email);
        bool CheckPassword(User user, string password);
        Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken);
        Task<UserResponse> UpdateAsync(int id, UserRequest request);
    }
}
