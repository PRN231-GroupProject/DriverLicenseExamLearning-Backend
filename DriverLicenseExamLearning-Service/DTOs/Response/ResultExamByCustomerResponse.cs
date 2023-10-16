﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.DTOs.Response
{
    public class ResultExamByCustomerResponse
    {
        public int QuizID { get; set; }
        public string? QuizName { get; set; }
        public int Mark { get; set; }
        public IQueryable<ResultExamDetailByCustomerResponse>? resultExamDetails { get; set; }


    }

    public class ResultExamDetailByCustomerResponse : QuestionGetByMemberResponse
    {
        public string? UserAnswer { get; set; }

        public string? RightAnswer { get; set; }

    }
}
