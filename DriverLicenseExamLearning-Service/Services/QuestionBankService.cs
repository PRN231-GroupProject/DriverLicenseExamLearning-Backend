using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.Ultilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.Services
{
    
    public interface IQuestionBankService
    {
        Task<bool> AddQuizRequests(List<AddQuestionRequest> requests);


        Task<bool> UpdateQuizRequests(int quizID, AddQuestionRequest request);

        Task<IQueryable<QuestionBankResponse>> QuestionBank();
    }
    public class QuestionBankService : IQuestionBankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionBankService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> AddQuizRequests(List<AddQuestionRequest> requests)
        {
            int checkErrorQuiz = 0;
            List<string> errors = new List<string>();
            #region "Add New Quiz And Check"
            foreach (var item in requests)
            {
                bool check = await CheckQuiz(item.Answer, new string[4] { item.Options1, item.Options2, item.Options3, item.Options4 });
                if (check)
                {
                    var quizAdd = _mapper.Map<Question>(item);

                    await _unitOfWork.Repository<Question>().CreateAsync(quizAdd);
                    _unitOfWork.Commit();
                }
                else
                {
                    errors.Add($"Have problem with question : {item.Text}");
                    checkErrorQuiz++;

                }
            }
            #endregion

            if(checkErrorQuiz > 0)
            {
                throw  new CrudException<List<string>>(HttpStatusCode.BadRequest, "Have problem when add new questions to bank", errors);
            }


            return true;
            
        }

        public async Task<IQueryable<QuestionBankResponse>> QuestionBank() => await QueryFormat.QueryQuestionFollowLisenceType();
     

     

        public async Task<bool> UpdateQuizRequests(int quizID, AddQuestionRequest request)
        {
           var questionObject = _mapper.Map<Question>(request);

             await _unitOfWork.Repository<Question>().Update(questionObject,quizID);
             _unitOfWork.Commit();
            return true;
        }


        private async Task<bool> CheckQuiz(string answer, string[] optionList)
        {
            int valueCheck = 0;
            foreach (var item in optionList)
            {
                if (item.Equals(answer))
                {
                    valueCheck++;
                }
            }
            return valueCheck == 1;
        }
    }
}
