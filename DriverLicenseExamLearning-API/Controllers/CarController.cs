using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.State;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace DriverLicenseExamLearning_API.Controllers
{
    [Route("api/car")]
    public class CarController : ODataController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [Authorize]
        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var car = await _carService.GetCar();
            if(car is null)
            {
                return BadRequest(new {
                message = "Empty"});
            }
            return Ok(car);

        }

        [HttpDelete]
        [Authorize(Roles = RoleNames.Staff)]
        [SwaggerOperation(Summary = $"[Role :{RoleNames.Staff}][Description:Delete  Car]")]
        public async Task<ActionResult> Delete(int CarID)
        {
            bool checkTrue = await _carService.DeleteCar(CarID);
            if (!checkTrue)
            {
                return BadRequest(new
                {
                    message = "Delete Unsecessfully"
                }
                );
            }

            return Ok(new
            {
                message = "Delete Sucessfully"
            });
        }

        [Authorize(Roles = RoleNames.Staff)]
        [SwaggerOperation(Summary = $"[Role : {RoleNames.Staff}][Description:Update  Car]")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UpdateCarRequest car, int carID)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            bool check = await _carService.UpdateCar(car, carID);
            if (!check)
            {
                return BadRequest(new
                {
                    message = "Update Unsecessfully"
                });
            }

            return Ok(new
            {
                message = "Update Sucessfully"
            });
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Staff)]
        [SwaggerOperation(Summary = $"[Role : {RoleNames.Staff}][Description:Add new  Car]")]
        public async Task<IActionResult> Add([FromBody] UpdateCarRequest carRequest)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var rs = await _carService.CreateCar(carRequest);
            return Ok(new
            {
                message = "Create Sucessfully"
            });
        }
    }
}
