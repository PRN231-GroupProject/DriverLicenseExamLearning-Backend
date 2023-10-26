using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface ITrackingService
    {
        Task<IEnumerable<Tracking>> GetAllAsync();
        Task<bool> CreateTrackingByBookingId(int bookingId, TrackingRequest request);
    }
}
