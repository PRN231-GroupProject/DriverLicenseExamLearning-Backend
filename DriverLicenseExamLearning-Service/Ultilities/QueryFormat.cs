using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.Ultilities
{
    
    public class QueryFormat
    {
        private static DriverLicenseExamLearningContext _context = new DriverLicenseExamLearningContext();


        public static async Task<IQueryable<QuestionBankResponse>>  QueryQuestionFollowLisenceType()
        {
            IQueryable<QuestionBankResponse> result =  (from lisencetype in _context.LicenseTypes
                          join question in _context.Questions on lisencetype.LicenseTypeId equals question.LicenseTypeId
                          select new QuestionBankResponse
                          {
                              LicenseTypeID = lisencetype.LicenseTypeId,
                              Name = lisencetype.Name,
                              questions = (from on1 in _context.Questions
                                           where on1.LicenseTypeId == lisencetype.LicenseTypeId
                                           select new AddQuestionRequest
                                           {
                                               Text = on1.Text,
                                                Options4 = on1.Options4,
                                                Answer = on1.Answer,
                                                Options1 = on1.Options1,
                                                Options2 = on1.Options2,
                                                Options3 = on1.Options3,


                                           }).ToList()


                          }
                          ).AsQueryable();
            return result;
        
        }



        public static async Task<IQueryable<ExamQueryGeneralResponse>> QueryExamFollowLisenceType()
        {
            IQueryable<ExamQueryGeneralResponse> result = (from LicenseType in _context.LicenseTypes
                                                           join exam in _context.Exams on LicenseType.LicenseTypeId equals exam.LicenseTypeId
                                                           select new ExamQueryGeneralResponse { LicenseTypeId = LicenseType.LicenseTypeId, Name = LicenseType.Name
                                                           , examQueries = (from li in _context.LicenseTypes
                                                                            join ex in _context.Exams on li.LicenseTypeId equals ex.LicenseTypeId
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
                                                                                                    Options1 = q.Options1,
                                                                                                    Options2 =q.Options2,
                                                                                                    Options3 = q.Options3,
                                                                                                    Options4 = q.Options4,
                                                                                                    Text = q.Text
                                                                                                }
                                                                                               ).ToList()

                                                                            }).ToList()
                                                           }).AsQueryable();
        
                
            return result;
        }

    }
}
