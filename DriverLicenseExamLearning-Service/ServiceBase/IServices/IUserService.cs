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
        Task<bool> DeleteUser(int userID);

        Task<bool> UpdateUser(int userID, UserLoginRequest user);
    }
}
