using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/Transaction")]
    public class TransactionController : ODataController
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [Authorize(Roles = RoleNames.Staff)]
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<TransactionResponse>> GetAllTransaction()
        {
            var rs = await _transactionService.GetAllAsync();
            return rs != null ? Ok(rs) : NotFound();
        }
        [Authorize(Roles = RoleNames.Member)]
        [HttpPost("transaction")]
        public async Task<ActionResult> CreateTransaction(int bookingId, [FromBody] TransactionRequest req)
        {
            var rs = await _transactionService.CreateTransactionByBookingId(bookingId, req);
            return rs != null ? Ok(new
            {
                msg = "Create Transaction Successfully!"
            }) : BadRequest(new
            {
                msg = "Create transaction fail!"
            });
        }
        [Authorize(Roles = RoleNames.Member)]
        [HttpPost("refund")]
        public async Task<ActionResult> CreateRefund(int bookingId, [FromBody] TransactionRequest req)
        {
            var rs = await _transactionService.RefundTransactionByBookingId(bookingId, req);
            return rs == true ? Ok(new
            {
                msg = "Waiting for the dev to refund your money!"
            }) : BadRequest(new
            {
                msg = "Refund fail!"
            });
        }
        [Authorize(Roles = RoleNames.Mentor)]
        [HttpPut("{transactionId:int}")]
        public async Task<ActionResult> UpdateTransaction(int transactionId, [FromBody] TransactionRequest req)
        {
            var rs = await _transactionService.UpdateTransaction(transactionId, req);
            return rs != null ? Ok(new
            {
                msg = "Update Successfully"
            }) : BadRequest(new
            {
                msg = "Update Fail!"
            });
        }
    }
}
