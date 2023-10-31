using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IClaimsService _claimsService;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<bool> CreateTransactionByBookingId(int bookingId, [FromBody] TransactionRequest request) //Pay - Receive - Refund
        {
            request.UserId = _claimsService.GetCurrentUserId;
            request.TransactionType = "Pay";
            request.Total = _unitOfWork.Repository<Booking>()
                                   .Include(x => x.Package)
                                   .Where(x => x.BookingId == bookingId)
                                   .Select(x => x.Package.Price).FirstOrDefault();
            request.Status = "Pending";
            request.BookingId = bookingId;

            var p = _mapper.Map<Transaction>(request);
            await _unitOfWork.Repository<Transaction>().CreateAsync(p);
            await _unitOfWork.CommitAsync();
            return true;
        }
        public async Task<bool> RefundTransactionByBookingId(int bookingId, [FromBody] TransactionRequest request)
        {
            request.UserId = _claimsService.GetCurrentUserId;
            request.TransactionType = "Refund";

            var totalMoneyInPackage = _unitOfWork.Repository<Booking>()
                                                 .Include(x => x.Package)
                                                 .Where(x => x.BookingId == bookingId)
                                                 .Select(x => x.Package.Price)
                                                 .FirstOrDefault();

            var numberOfDays = _unitOfWork.Repository<Booking>()
                                          .Include(x => x.Trackings)
                                          .Where(x => x.BookingId == bookingId)
                                          .Count();

            if (numberOfDays == 2) //1 day
            {
                request.Total = totalMoneyInPackage;
            }
            if (numberOfDays == 3) //2 days => 80% money
            {
                request.Total = totalMoneyInPackage*80/100;
            }
            if (numberOfDays > 3) //more than 3 days, no refund => quit
            {
                request.Total = 0;
                return false;
            }
            request.Status = "Pending";
            request.BookingId = bookingId;

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

        public async Task<bool> UpdateTransaction(int id, [FromBody] TransactionRequest request)
        {
            var check = await _unitOfWork.Repository<Transaction>().Where(x => x.TransactionId == id).FirstOrDefaultAsync();
            await _unitOfWork.Repository<Transaction>().Update(check, id);
            await _unitOfWork.CommitAsync();
            return true;
        }
    }
}
