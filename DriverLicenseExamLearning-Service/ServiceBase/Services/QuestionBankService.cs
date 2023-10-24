using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.State;
using DriverLicenseExamLearning_Service.Support.HandleError;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{


    public class QuestionBankService : IQuestionBankService
    {
         private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionBankService(IUnitOfWork unitOfWork, IMapper mapper,IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsService = claimsService;
        }
        public async Task<bool> AddQuizRequests(List<AddQuestionRequest> requests)
        {

            // Check role
         
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

            if (checkErrorQuiz > 0)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest,errors.ToString());
            }


            return true;

        }

        public async Task<IQueryable<QuestionBankResponse>> QuestionBank() => await QueryFormat.QueryQuestionFollowLisenceType();




        public async Task<bool> UpdateQuizRequests(int quizID, AddQuestionRequest request)
        {
            Question questionFind = _unitOfWork.Repository<Question>().Where(x => x.QuestionId == quizID).FirstOrDefault();
           if(request.Options4  != null && !request.Options4.Equals("string"))
            {
                questionFind.Option4 = request.Options4;
            }
            if (request.Options3 != null && !request.Options3.Equals("string"))
            {
                questionFind.Option3 = request.Options3;
            }
            if (request.Options1 != null && !request.Options1.Equals("string"))
            {
                questionFind.Option1 = request.Options1;
            }
            if (request.Options2 != null && !request.Options2.Equals("string"))
            {
                questionFind.Option2 = request.Options2;
            }
            if (request.Text != null && !request.Text.Equals("string"))
            {
                questionFind.Question1 = request.Text;
            }
            if(request.Image != null && !request.Image.Equals("string"))
            {
                questionFind.Image = request.Image;
            }
            await _unitOfWork.Repository<Question>().Update(questionFind, quizID);
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
