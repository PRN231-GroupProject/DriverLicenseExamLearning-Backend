using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.HandleError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class MemberDayRegisterService : IMemberDayRegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClaimsService _claimsService;
        public MemberDayRegisterService(IUnitOfWork unitOfWork, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimsService = claimsService;
        }

        public bool checkDate(string date)
        {
            DateTime parsedDate;
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
        }

        public async Task<IEnumerable<MemberDayRegisterResponse>> CreateMemberDayRegisterByBookingId(int bookingId, MemberDayRegisterRequest req)
        {
            var userId = _claimsService.GetCurrentUserId;
            var memberDayRegisterResponses = new List<MemberDayRegisterResponse>();
            DateTime? lastDate = null;

            foreach (var date in req.Dates)
            {
                #region Hyper Date Validate
                var booking = _unitOfWork.Repository<Booking>()
                                     .Include(x => x.Trackings)
                                     .Where(x => x.BookingId == bookingId)
                                     .Select(x => new
                                     {
                                         TrackingsType = x.Trackings.FirstOrDefault().Type
                                     })
                                     .FirstOrDefault();

                var memberId = _unitOfWork.Repository<Booking>()
                                          .Where(x => x.BookingId == bookingId)
                                          .Select(x => x.MemberId)
                                          .FirstOrDefault();

                if (userId != memberId)
                {
                    throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "You cannot register others people days!!!");
                }

                if (booking.TrackingsType == "Days")
                {
                    int? totalDaysFromTracking = _unitOfWork.Repository<Tracking>()
                                                           .Where(x => x.BookingId == bookingId)
                                                           .Sum(x => x.Total);

                    int totalDaysInRequest = req.Dates.Count;

                    if (totalDaysFromTracking != totalDaysInRequest)
                    {
                        throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Total days does not match the number of days in package!");
                    }
                }

                if (lastDate.HasValue && date.Date == lastDate)
                {
                    throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Date cannot be the same!");
                }

                if (date < DateTime.UtcNow.AddHours(7))
                {
                    throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "You cannot choose date in the past!");
                }

                if (lastDate.HasValue && (date - lastDate.Value).TotalDays > 7)
                {
                    throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Dates must be within 1 week of each others!");
                }

                if (lastDate.HasValue && (date - req.Dates.First()).TotalDays > 365)
                {
                    throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "The first date and the last date is not more than 1 years!");
                }
                #endregion

                var newMemberDayRegister = new MemberDayRegister
                {
                    BookingId = bookingId,
                    Datetime = date
                };

                await _unitOfWork.Repository<MemberDayRegister>().CreateAsync(newMemberDayRegister);

                var response = new MemberDayRegisterResponse
                {
                    BookingId = bookingId,
                    Dates = new List<DateTime> { date }
                };

                memberDayRegisterResponses.Add(response);
                lastDate = date;
            }

            await _unitOfWork.CommitAsync();
            return memberDayRegisterResponses;
        }

        public async Task<MemberDayRegisterResponse> GetMemberDayRegisterByBookId(int bookId)
        {
            var memberDayRegisters = await _unitOfWork.Repository<MemberDayRegister>()
                .Where(mdr => mdr.BookingId == bookId)
                .ToListAsync();

            var memberDayRegisterResponse = new MemberDayRegisterResponse
            {
                BookingId = bookId,
                Dates = memberDayRegisters.Select(mdr => mdr.Datetime.Value).ToList()
            };

            return memberDayRegisterResponse;
        }
    }
}
