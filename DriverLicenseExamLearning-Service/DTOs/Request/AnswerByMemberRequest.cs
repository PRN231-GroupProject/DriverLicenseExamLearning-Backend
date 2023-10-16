using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public class AnswerByMemberRequest
    {
        public int QuizID { get; set; }

        public  IQueryable<AnswerDetailMemberRequest> answerDetails { get; set; }
    }


    public class AnswerDetailMemberRequest
    {
        public string? Answer { get; set; }

        public int QuestionID { get; set; }
    }
}
