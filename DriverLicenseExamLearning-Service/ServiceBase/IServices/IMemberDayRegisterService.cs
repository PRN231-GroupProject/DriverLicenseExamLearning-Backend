using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IMemberDayRegisterService
    {
        Task<IEnumerable<MemberDayRegisterResponse>> CreateMemberDayRegisterByBookingId(int booking, MemberDayRegisterRequest req);
        Task<MemberDayRegisterResponse> GetMemberDayRegisterByBookId(int bookId);
        bool checkDate(string date);
    }
}
