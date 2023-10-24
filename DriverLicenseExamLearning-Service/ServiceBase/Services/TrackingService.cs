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
            var existingTrackings = await _unitOfWork.Repository<Tracking>().GetWhere(x => x.BookingId == bookingId);

            int? totalProcessing = existingTrackings.Sum(tracking => tracking.Processing);
            request.Total = existingTrackings.FirstOrDefault().Total; //add the first value (0)

            if (totalProcessing + request.Processing < request.Total)
            {
                request.Type = existingTrackings.First().Type;
                if(request.Type == "Days")
                {
                    request.Processing = 1;
                }

                var newTracking = _mapper.Map<Tracking>(request);
                newTracking.BookingId = bookingId;

                await _unitOfWork.Repository<Tracking>().CreateAsync(newTracking);
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
