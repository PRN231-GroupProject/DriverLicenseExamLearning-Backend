using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class MarkResultResponse
    {
        public int WrongAnswer { get ; set; }

        public int RightAnswer { get; set; }

        public string? Mark { get; set; }

        public int WrongParalysisAnswer { get; set; }
        public string? ExamStatus { get; set; }
    }
}
