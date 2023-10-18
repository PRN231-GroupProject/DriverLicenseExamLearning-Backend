﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IBookingService
    {
        Task<bool> CreateBooking(BookingRequest req);
        Task<bool> UpdateBooking(BookingRequest req);
        Task<IQueryable<BookingResponse>> GetAllBooking();
    }
}
