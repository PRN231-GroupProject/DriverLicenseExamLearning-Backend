using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface IMentorApplication
    {
        Task<IEnumerable<MentorApplicationResponse>> GetMentorApplications();

        Task<bool> UpdateMentorApplication(MentorApplicationRequest request);
    }
}
