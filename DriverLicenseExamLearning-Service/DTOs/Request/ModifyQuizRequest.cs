using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public struct ModifyQuizRequest
    {
        public List<int>? RemoveFromQuiz { get; set; } 

        public List<int>? AddToQuiz { get; set; } 


    }
}
