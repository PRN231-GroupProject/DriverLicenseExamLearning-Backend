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
    }
}
