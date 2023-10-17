using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class LicenseTypeService : ILicenseTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LicenseTypeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<LicenseType>> GetAllAsync() => await _unitOfWork.Repository<LicenseType>().GetAllAsync();
    }
}
