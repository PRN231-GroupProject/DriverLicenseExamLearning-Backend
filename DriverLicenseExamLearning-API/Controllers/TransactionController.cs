using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public async Task<ActionResult<TransactionResponse>> GetAllTransaction()
        {
            var rs = await _transactionService.GetAllAsync();
            return rs != null ? Ok(rs) : NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] TransactionRequest req)
        {
            var rs = await _transactionService.CreateTransaction(req);
            return rs != null ? Ok(new
            {
                msg = "Create Transaction Successfully!"
            }) : BadRequest(new
            {
                msg = "Create transaction fail!"
            });
        }
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
