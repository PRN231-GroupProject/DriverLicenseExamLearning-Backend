using DriverLicenseExamLearning_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class ExamGetByMemberResponse
    {
        public int ExamId { get; set; }

        public string ExamName { get; set; }

        public DateTime ExamDate { get; set; }

        public IQueryable<QuestionGetByMemberResponse> questions { get; set; }

    }



public class QuestionGetByMemberResponse
{
    public int QuestionId { get; set; }

    public string? Image { get; set; }


    public string? Title { get; set; }

    public string? Option1 { get; set; }

    public string? Option2 { get; set; }


    public string? Option3 { get; set; }

    public string? Option4 { get; set; }


}


}
