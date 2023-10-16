using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Request
{
    public  class CreateNewExamRequest
    {
        public string? Status { get; set; }
        public string? ExamName { get; set; }

        public int? LicenseTypeId { get; set; }

        public List<int>? QuestionID { get; set; }




    }
}
