using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.Support.Ultilities
{

    public class QueryFormat
    {
        private static PRN231_DriverLicenseExamLearningContext _context = new PRN231_DriverLicenseExamLearningContext();



   
        public static async Task<int> CheckMemberInBooking(int examId, int memberId)
        {
            int licenseTypeID = 0;

            var result = await (from exam in _context.Exams
                                join licenseType in _context.LicenseTypes on exam.LicenseId equals licenseType.LicenseTypeId
                                join package in _context.Packages on licenseType.LicenseTypeId equals package.LicenseTypeId
                                join booking in _context.Bookings on package.PackageTypeId equals booking.PackageId
                                where booking.MemberId == memberId && exam.ExamId == examId
                                select licenseType.LicenseTypeId).FirstOrDefaultAsync(); // Use FirstOrDefault to avoid exceptions
            if (result != 0)
            {
                licenseTypeID = result;
            }
            return licenseTypeID;
        }
    }
}
