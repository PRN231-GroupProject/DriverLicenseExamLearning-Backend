using AutoMapper;
using AutoMapper.QueryableExtensions;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.Helpers;
using DriverLicenseExamLearning_Service.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.Services
{
    public interface IUserService
    {
        Task<PagedResult<UserResponse>> GetCustomers(UserRequest request, PagingRequest paging);
        bool CheckRegexEmail(string email);
        Task<IEnumerable<User>> GetAllAsync();
    }
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
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            return users;
        }

        public Task<PagedResult<UserResponse>> GetCustomers(UserRequest request, PagingRequest paging)
        {
            var filter = _mapper.Map<UserResponse>(request);
            var customer = _unitOfWork.Repository<User>()
                                      .GetAll().Where(x => x.RoleId != 1)
                                      .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                      .DynamicFilter(filter)
                                      .ToList();
            var sort = PageHelper<UserResponse>.Sorting(paging.SortType, customer, paging.ColName);
            var result = PageHelper<UserResponse>.Paging(sort, paging.Page, paging.PageSize);
            return Task.FromResult(result);
        }
    }
}
