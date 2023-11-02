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



        public static async Task<IEnumerable<ResultExamByCustomerResponse>> GetHistoryExam(int licenseID, int userID)
        {
            IEnumerable<ResultExamByCustomerResponse> exams = (from license in _context.LicenseTypes
                         join exam in _context.Exams on license.LicenseTypeId equals exam.LicenseId
                         join examResult in _context.ExamResults on exam.ExamId equals examResult.ExamId
                         where license.LicenseTypeId == licenseID && examResult.UserId == userID
                         select new ResultExamByCustomerResponse
                         {
                             ResultExamId = examResult.ExamResultId,
                             QuizName = exam.ExamName,
                             Mark = examResult.Result,
                             QuizID = exam.ExamId,
                             resultExamDetails = (from examresultdetail in _context.ExamResultDetails 
                                                  join examquestion in _context.ExamQuestions on examResult.ExamId equals examquestion.ExamId
                                                  join question in _context.Questions on examquestion.QuestionId equals question.QuestionId
                                                  where examresultdetail.ExamResultId == examResult.ExamResultId && examquestion.ExamId ==  exam.ExamId
                                                  select new ResultExamDetailByCustomerResponse
                                                  {
                                                      Option1 = question.Option1,
                                                      QuestionId = question.QuestionId,
                                                      Option2 = question.Option2,
                                                      Image = question.Image,
                                                      Option3 = question.Option3,
                                                      Option4 = question.Option4,
                                                      RightAnswer = question.Answer,
                                                      UserAnswer = examresultdetail.WrongAnswer != null ? examresultdetail.WrongAnswer : question.Answer,
                                                      
                                                  })
                                                  .GroupBy(x => x.QuestionId)
                                                  .Select( x => x.First() )
                                                  .ToHashSet()


                         }).AsEnumerable();

            return exams;
        }
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
