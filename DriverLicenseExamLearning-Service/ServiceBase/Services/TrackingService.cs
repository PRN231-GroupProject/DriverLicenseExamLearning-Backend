using DriverLicenseExamLearning_Data.Entity;
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
        public Task<bool> CreateTransaction(TrackingRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTransaction(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tracking>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTransaction(int id, TrackingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
