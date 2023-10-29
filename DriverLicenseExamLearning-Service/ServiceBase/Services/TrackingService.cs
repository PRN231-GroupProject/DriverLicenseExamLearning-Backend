using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
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
        private readonly IClaimsService _claimsService;
        public TrackingService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }
        public async Task<bool> CreateTrackingByBookingId(int bookingId, TrackingRequest request)
        {
            var existingTrackings = await _unitOfWork.Repository<Tracking>().GetWhere(x => x.BookingId == bookingId);

            int ? totalProcessing = existingTrackings.Sum(tracking => tracking.Processing);
            request.Total = existingTrackings.FirstOrDefault().Total;
            request.Type = existingTrackings.First().Type;

            //if (request.Type == "Km")
            //{
            //    request.Total += 1;
            //}

            if (totalProcessing + request.Processing <= request.Total)
            {
                if(request.Type == "Days")
                {
                    request.Processing = 1;
                }

                //if (request.Type == "Km")
                //{
                //    request.Total -= 1;
                //}

                var newTracking = _mapper.Map<Tracking>(request);
                newTracking.BookingId = bookingId;

                await _unitOfWork.Repository<Tracking>().CreateAsync(newTracking);

                if (totalProcessing + request.Processing == request.Total)
                {
                    #region Change Status Car
                    var existingBookings = await _unitOfWork.Repository<Booking>().FindAsync(x => x.BookingId == bookingId);
                    int? carId = existingBookings.CarId;
                    var existingCars = await _unitOfWork.Repository<Car>().FindAsync(x => x.CarId == carId);
                    existingCars.Status = "Active";
                    await _unitOfWork.Repository<Car>().Update(existingCars, carId.Value);
                    #endregion

                    var mentorId = _claimsService.GetCurrentUserId;
                    var existingMentors = await _unitOfWork.Repository<User>().FindAsync(x => x.UserId == mentorId && x.Status == "Not Available");
                    existingMentors.Status = "Available";
                    await _unitOfWork.Repository<User>().Update(existingMentors, mentorId);
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<Tracking>> GetAllAsync() => await _unitOfWork.Repository<Tracking>().GetAllAsync();
    }
}
