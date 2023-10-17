using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
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


                                                                        }).ToList()


                                                       }
                          ).AsQueryable();
            return result;

        }



        public static async Task<IQueryable<ExamQueryGeneralResponse>> QueryExamFollowLisenceType()
        {
            IQueryable<ExamQueryGeneralResponse> result = (from LicenseType in _context.LicenseTypes
                                                           join exam in _context.Exams on LicenseType.LicenseTypeId equals exam.LicenseId
                                                           select new ExamQueryGeneralResponse
                                                           {
                                                               LicenseTypeId = LicenseType.LicenseTypeId,
                                                               Name = LicenseType.LicenseName
                                                           ,
                                                               examQueries = (from li in _context.LicenseTypes
                                                                              join ex in _context.Exams on li.LicenseTypeId equals ex.LicenseId
                                                                              select new ExamQueryResponse
                                                                              {
                                                                                  Date = (DateTime)ex.ExamDate,
                                                                                  ExamId = ex.ExamId,
                                                                                  examDetails = (from eq in _context.ExamQuestions
                                                                                                 join q in _context.Questions on eq.QuestionId equals q.QuestionId
                                                                                                 where eq.ExamId == ex.ExamId
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
                                                           }).AsQueryable();


            return result;
        }

    }
}
