﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IQuestionBankService
    {
        Task<bool> AddQuizRequests(List<AddQuestionRequest> requests);

        Task<bool> UpdateQuizRequests(int quizID, AddQuestionRequest request);

        Task<IEnumerable<QuestionBankResponse>> QuestionBank();
    }
}
