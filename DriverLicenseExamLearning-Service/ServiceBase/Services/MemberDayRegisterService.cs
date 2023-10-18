using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class MemberDayRegisterService : IMemberDayRegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MemberDayRegisterService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MemberDayRegisterResponse>> CreateMemberDayRegisterByBookingId(int bookingId, MemberDayRegisterRequest req)
        {
            var memberDayRegisterResponses = new List<MemberDayRegisterResponse>();

            foreach (var date in req.Dates)
            {
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
