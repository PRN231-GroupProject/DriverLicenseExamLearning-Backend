using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrackingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    
        public async Task<bool> CreateTransactionByBookingId(int bookingId, TrackingRequest request)
        {
            var check = _unitOfWork.Repository<Booking>().GetById(bookingId);
            request.BookingId = check.Id;
            var booking = _mapper.Map<Tracking>(request);
            await _unitOfWork.Repository<Tracking>().CreateAsync(booking);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Task<IEnumerable<Tracking>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
