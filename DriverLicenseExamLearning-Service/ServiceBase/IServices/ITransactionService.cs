using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices {
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<bool> DeleteTransaction(int id);
        Task<bool> CreateTransactionByBookingId(int bookingId, TransactionRequest request);
        Task<bool> UpdateTransaction(int id, TransactionRequest request);
        Task<bool> RefundTransactionByBookingId(int bookingId, [FromBody] TransactionRequest request);
    }
}
