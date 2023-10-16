using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Service.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.IServices
{
    public  interface ICarService
    {
        public Task<IQueryable<Car>> GetCar();

        public Task<bool> UpdateCar(UpdateCarRequest  car,int carID);

        public Task<bool> DeleteCar(int carID);

        public Task<bool> CreateCar(UpdateCarRequest newCar);
    }
}
