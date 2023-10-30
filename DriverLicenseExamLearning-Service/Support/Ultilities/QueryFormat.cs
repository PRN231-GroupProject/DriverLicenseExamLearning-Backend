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


        public static async Task<IQueryable<QuestionBankResponse>> QueryQuestionFollowLisenceType()
        {
           
            IQueryable<QuestionBankResponse> result = (from lisencetype in _context.LicenseTypes
                                                       join question in _context.Questions on lisencetype.LicenseTypeId equals question.LicenseType
                                                       select new QuestionBankResponse
                                                       {
                                                           LicenseTypeID = lisencetype.LicenseTypeId,
                                                           Name = lisencetype.LicenseName,
                                                           questions = (from on1 in _context.Questions
                                                                        where on1.LicenseType == lisencetype.LicenseTypeId
                                                                        select new AddQuestionRequest
                                                                        {
                                                                            Text = on1.Question1,
                                                                            Options4 = on1.Option4,
                                                                            Answer = on1.Answer,
                                                                            Options1 = on1.Option1,
                                                                            Options2 = on1.Option2,
                                                                            Options3 = on1.Option3,
                                                                            LicenseTypeId = lisencetype.LicenseTypeId
                                                                            ,
                                                                            Image = on1.Image != null ? on1.Image : "Not have picture",
                                                                            ParalysisQuestion = on1.IsParalysisQuestion

                                                                        }).ToList()


                                                       }
                                                )
                                               .GroupBy(x => x.LicenseTypeID)
                                                 .Select(x => x.First())
                                                .AsQueryable();
            return result;

        }



        public static async Task<IQueryable<ExamQueryGeneralResponse>> QueryExamFollowLisenceTypeGetByStaff()
        {
            IQueryable<ExamQueryGeneralResponse> result = (from LicenseType in _context.LicenseTypes
                                                           select new ExamQueryGeneralResponse
                                                           {
                                                               LicenseTypeId = LicenseType.LicenseTypeId,
                                                               Name = LicenseType.LicenseName,
                                                               examQueries = (from li in _context.LicenseTypes
                                                                              join ex in _context.Exams on li.LicenseTypeId equals ex.LicenseId
                                                                              where li.LicenseTypeId == LicenseType.LicenseTypeId
                                                                              select new ExamQueryResponse
                                                                              {
                                                                                  ExamName = ex.ExamName,
                                                                                  Date = (DateTime)ex.ExamDate,
                                                                                  ExamId = ex.ExamId,
                                                                                  examDetails = (from eq in _context.ExamQuestions
                                                                                                 join q in _context.Questions on eq.QuestionId equals q.QuestionId
                                                                                                 where eq.ExamId == ex.ExamId && eq.ExamId == ex.ExamId
                                                                                                 select new ExamDetailResponse
                                                                                                 {
                                                                                                     Answer = q.Answer,
                                                                                                     Options1 = q.Option1,
                                                                                                     Options2 = q.Option2,
                                                                                                     Options3 = q.Option3,
                                                                                                     Options4 = q.Option4,
                                                                                                     Text = q.Question1
                                                                                                 }
                                                                                                 ).ToList()

                                                                              }).ToList()
                                                           })
                                                           .AsQueryable();


            return result;
        }


        public static async Task<IQueryable<ExamGetByLicenseTye>> QueryExamFollowLisenceTypeByMember(int? licenseTypeID)
        {

            
            IQueryable<ExamGetByLicenseTye> result = (from licensetype in _context.LicenseTypes
                                                      where licensetype.LicenseTypeId == licenseTypeID
                                                      select new ExamGetByLicenseTye
                                                      {
                                                          LicenseId = licensetype.LicenseTypeId,
                                                          LicenseName = licensetype.LicenseName,
                                                          exams = (from lct in _context.LicenseTypes
                                                                   join exam in _context.Exams on lct.LicenseTypeId equals exam.LicenseId
                                                                   where lct.LicenseTypeId == licensetype.LicenseTypeId
                                                                   select new ExamGetByMemberResponse
                                                                   {
                                                                       ExamDate = (DateTime)exam.ExamDate,
                                                                       ExamId = exam.ExamId,
                                                                       ExamName = exam.ExamName,
                                                                       questions = (from eq in _context.ExamQuestions
                                                                                    join q in _context.Questions on eq.QuestionId equals q.QuestionId
                                                                                    where eq.ExamId == exam.ExamId
                                                                                    select new QuestionGetByMemberResponse
                                                                                    {
                                                                                        Image = q.Image,
                                                                                        Option1 = q.Option1,
                                                                                        Option2 = q.Option2,
                                                                                        Option3 = q.Option3,
                                                                                        Option4 = q.Option4,
                                                                                        Title = q.Question1,
                                                                                        QuestionId = q.QuestionId,
                                                                                    }
                                                                          ).ToList()
                                                                   }).ToList()
                                                      }).GroupBy(x => x.LicenseId)
                                                      .Select(x => x.First()).AsQueryable();

            return result;
        }


        public static async Task<IQueryable<ResultExamByCustomerResponse>> GetHistoryExam(int licenseID, int userID)
        {
            IQueryable<ResultExamByCustomerResponse> exams = (from license in _context.LicenseTypes
                                                              join exam in _context.Exams on license.LicenseTypeId equals exam.LicenseId
                                                              join examResult in _context.ExamResults on exam.ExamId equals examResult.ExamId
                                                              where license.LicenseTypeId == licenseID && examResult.UserId == userID
                                                              select new ResultExamByCustomerResponse
                                                              {
                                                                  QuizName = exam.ExamName,
                                                                  Mark = examResult.Result,
                                                                  QuizID = exam.ExamId,
                                                                  resultExamDetails = (from examDetailResult in _context.ExamResultDetails
                                                                                       join question in _context.Questions on examDetailResult.QuestionId equals question.QuestionId
                                                                                       select new ResultExamDetailByCustomerResponse
                                                                                       {
                                                                                           Option1 = question.Option1,
                                                                                           QuestionId = question.QuestionId,
                                                                                           Option2 = question.Option2,
                                                                                           Image = question.Image,
                                                                                           Option3 = question.Option3,
                                                                                           Option4 = question.Option4,
                                                                                           RightAnswer = question.Answer,
                                                                                           UserAnswer = examDetailResult.WrongAnswer != null ? examDetailResult.WrongAnswer : question.Answer,
                                                                                       })
                                                                                       .GroupBy(result => result.QuestionId)
                                                                                       .Select(x => x.First())
                                                                                       .ToHashSet()


                                                              }).AsQueryable();

            return exams;
        }


        public static async Task<IQueryable<LicenseApplicationResponse>> GetLicenseApplicationByStaff()
        {
            var result = (from licenseapplication in _context.LicenseApplications
                          join user in _context.Users on licenseapplication.UserId equals user.UserId
                          select new LicenseApplicationResponse
                          {
                              userId = user.UserId,
                              loader = ((IReadOnlyCollection<LicenseApplicationDetailResponse>)(from LicenseApplication in _context.LicenseApplications
                                                                                                where LicenseApplication.LicenseApplicationId == licenseapplication.LicenseApplicationId
                                                                                                select new LicenseApplicationDetailResponse
                                                                                                {
                                                                                                    CitizenIdentificationCard = LicenseApplication.CitizenIdentificationCard,
                                                                                                    CurriculumVitae = LicenseApplication.CurriculumVitae,
                                                                                                    HealthCertification = LicenseApplication.HealthCertification,
                                                                                                    UserImage = licenseapplication.UserImage,
                                                                                                    LicenseTypeID = licenseapplication.LicenseTypeId,
                                                                                                    Status = licenseapplication.Status

                                                                                                })).ToList()


                          }).AsQueryable();


            return result;
        }



        public static async Task<IQueryable<LicenseApplicationDetailResponse>> GetLicenseApplicationByCustomer(uint customerId)
        {
            var result = (from licenseapplication in _context.LicenseApplications
                          join user in _context.Users on licenseapplication.UserId equals user.UserId
                          where user.UserId == customerId
                          select new LicenseApplicationDetailResponse
                          {
                              CitizenIdentificationCard = licenseapplication.CitizenIdentificationCard,
                              CurriculumVitae = licenseapplication.CurriculumVitae,
                              HealthCertification = licenseapplication.HealthCertification,
                              LicenseTypeID = licenseapplication.LicenseTypeId,
                              UserImage = licenseapplication.UserImage,
                              Status = licenseapplication.Status
                          }).AsQueryable();


            return result;
        }


        public static async Task<int> CheckMemberInBooking(int examId, int memberId)
        {
            int licenseTypeID = 0;

            var result = (from exam in _context.Exams
                          join licenseType in _context.LicenseTypes on exam.LicenseId equals licenseType.LicenseTypeId
                          join package in _context.Packages on licenseType.LicenseTypeId equals package.LicenseTypeId
                          join booking in _context.Bookings on package.PackageTypeId equals booking.PackageId
                          join transaction in _context.Transactions on booking.BookingId equals transaction.BookingId
                          join user in _context.Users on transaction.UserId equals user.UserId
                          where user.UserId == memberId && exam.ExamId == examId
                          select licenseType.LicenseTypeId).FirstOrDefault(); // Use FirstOrDefault to avoid exceptions

            if (result != 0)
            {
                licenseTypeID = result;
            }

            return licenseTypeID;
        }
    }
}
