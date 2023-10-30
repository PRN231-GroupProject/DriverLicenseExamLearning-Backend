﻿using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IExamService
    {
        // Create New Exam by Staff
        //Task<bool> CreateNewExam(CreateNewExamRequest create);
       //Modify Exam by Staff
        Task<bool> ModifiedExam(uint quizID,ModifyQuizRequest modify);

        // Update Status Exam by Staff
        Task<bool> ChangeStatusExam(int examID, string status);

        //Get All Exam Query by Staff
        Task<IQueryable<ExamQueryGeneralResponse>> GetExamQuery();

        //Get Quiz by Customer
        Task<IQueryable<ExamGetByLicenseTye>> GetExamListByCustomer(int? licenseTypeID);

        //Doing quiz
        Task<string> DoingQuiz(AnswerByMemberRequest answer); 


        //View Exam History Doing follow licenseType
        Task<IQueryable<ResultExamByCustomerResponse>> GetExamHistory(int licenseTypeID);


        Task<bool> CreateExam(CreateNewExamRequest request);



       




    }
}
