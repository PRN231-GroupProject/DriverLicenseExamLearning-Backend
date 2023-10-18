﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public interface ILicenseApplicationService
    {

        Task<bool> GetAll();


        Task<bool> SubmitLicenseApplication(List<IFormFile> formFiles);

        Task<bool> UpdateLicenseApplicationByCustomer();

    }
}