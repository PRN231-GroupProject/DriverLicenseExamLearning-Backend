using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateBooking(BookingRequest req)
        {
            var newBooking = _mapper.Map<Booking>(req);
            await _unitOfWork.Repository<Booking>().CreateAsync(newBooking);
            await _unitOfWork.CommitAsync();
            return true;
        }

        public Task<IQueryable<BookingResponse>> GetAllBooking()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBooking(BookingRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
