using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    public class UsersController : ODataController
    {
        private readonly DriverLicenseExamLearningContext _db;

        public UsersController(DriverLicenseExamLearningContext db)
        {
            db = new DriverLicenseExamLearningContext();
        }
        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = _db.Users.ToList();
            return Ok(user);
        }
    }
}
