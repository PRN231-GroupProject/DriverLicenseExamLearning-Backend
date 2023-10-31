using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class QuestionBankResponse
    {
        public int LicenseTypeID { get; set; }

        public string? Name { get ; set; }

        public List<AddQuestionRequest>? questions { get; set; } = new List<AddQuestionRequest>();
    }
}
