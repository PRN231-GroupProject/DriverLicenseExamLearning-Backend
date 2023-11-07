using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;
using System.Threading.Tasks;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.Support.HandleError;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class MentorApplication : IMentorApplication
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IConfiguration _config;
        public MentorApplication(IUnitOfWork unitOfWork,IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }
        public async Task<IEnumerable<MentorApplicationResponse>> GetMentorApplications()
        {
            IEnumerable<MentorApplicationResponse> result = await _unitOfWork.Repository<MentorAttribute>().Include(x => x.User).Select(x => new MentorApplicationResponse
            {
                Address = x.User.Address,
                Bio = x.Bio,
                Email = x.User.Email,
                Experience = x.Experience,
                MentorName = x.User.UserName,
                Name = x.User.Name,
                PhoneNumber = x.User.PhoneNumber,
                Stauts = x.Status
            }).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateMentorApplication(MentorApplicationRequest request)
        {
            var customerFind = _unitOfWork.Repository<User>().Where(x => x.Email == request.Email).FirstOrDefault();
            if (customerFind is null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "Not Found this email");
            }
            var mentorAttributeFind = _unitOfWork.Repository<MentorAttribute>().Where(x => x.UserId == customerFind.UserId).FirstOrDefault();
            if (request.Status == "Denied")
            {
                mentorAttributeFind.Status = "Denied";
                await _unitOfWork.Repository<MentorAttribute>().Update(mentorAttributeFind, mentorAttributeFind.MentorAttributeId);
                _unitOfWork.Commit();
            }
            if (request.Status == "Accepted")
            {
                mentorAttributeFind.Status = "Accepted";
                await _unitOfWork.Repository<MentorAttribute>().Update(mentorAttributeFind, mentorAttributeFind.MentorAttributeId);
                customerFind.RoleId = 3;
                await _unitOfWork.Repository<User>().Update(customerFind, customerFind.UserId);
                _unitOfWork.Commit();
            }

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(request.Email);
            mail.From = new MailAddress(_config["SendMailAccount:UserName"].ToString(), "From Dryver", System.Text.Encoding.UTF8);
            mail.Subject = request.Subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = request.Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            var client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_config["SendMailAccount:UserName"], _config["SendMailAccount:AppPassword"]);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Send(mail);
            return true;
        }
    }
}
