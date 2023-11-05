using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
 

    public class ExamResultResponse
    {
        public int ExamId { get; set; }

        public string? ExamName { get; set; }

        public List<ExamResultDetailResponse>? Details { get; set; }
    }

    public class ExamResultDetailResponse
    {
        public int ExamResultId { get; set; }

        public int AttemptNumber { get; set; }

        public DateTime Date { get; set; }

        public string? Result { get; set; }


    }
}