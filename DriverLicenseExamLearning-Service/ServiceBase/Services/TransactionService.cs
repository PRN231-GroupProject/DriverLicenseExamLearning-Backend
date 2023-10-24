using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateTransaction(TransactionRequest request)
        {
            var p = _mapper.Map<Transaction>(request);
            await _unitOfWork.Repository<Transaction>().CreateAsync(p);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Task<bool> DeleteTransaction(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync() => await _unitOfWork.Repository<Transaction>().GetAllAsync();

        public async Task<bool> UpdateTransaction(int id, TransactionRequest request)
        {
            var check = await _unitOfWork.Repository<Transaction>().Where(x => x.TransactionId == id).FirstOrDefaultAsync();
            await _unitOfWork.Repository<Transaction>().Update(check, id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
