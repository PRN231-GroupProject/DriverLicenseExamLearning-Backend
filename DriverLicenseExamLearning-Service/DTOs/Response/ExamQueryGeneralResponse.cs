using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public struct ExamDetailResponse
    {
        public int QuestionId { get; set;  }
        public string Text { get; set; }
        public string Options1 { get; set; }
        public string Options2 { get; set; }
        public string Options3 { get; set; }
        public string Options4 { get; set; }
        public string Answer { get; set; }
    }


    public struct ExamQueryResponse
    {
        public string ExamName { get; set; }
        public int ExamId { get; set; }

        public DateTime Date  { get; set; }  
       public List<ExamDetailResponse> examDetails { get; set; }
    }


    public struct ExamQueryGeneralResponse
    {
        public int LicenseTypeId { get; set; }

        public string? Name { get; set; }

        public List<ExamQueryResponse>? examQueries { get; set; }
    }
}
