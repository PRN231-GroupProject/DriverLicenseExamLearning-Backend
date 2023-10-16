using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace DriverLicenseExamLearning_API.Controllers
{
    public class CarController  : ODataController
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var car  = await _carService.GetCar();
            return Ok(car);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int CarID)
        {
            bool checkTrue = await _carService.DeleteCar(CarID);
            if (checkTrue)
            {
                return BadRequest("Delete Unsecessfully") ;
            }

            return Ok("Delete Sucessfully");
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCarRequest car,int carID)
        {
           bool check = await _carService.UpdateCar(car, carID);
            if (!check)
            {
                return BadRequest("");
            }

            return Ok("");
        }

        [HttpPost]
        public async Task<IActionResult> Add(UpdateCarRequest carRequest)
        {
           await _carService.CreateCar(carRequest);
            return Ok("Create Sucessfully");
        }
    }
}
