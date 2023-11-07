using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.State;
using DriverLicenseExamLearning_Service.Support.HandleError;
using DriverLicenseExamLearning_Service.Support.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class CarService : ICarService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CarService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateCar(UpdateCarRequest newCar)
        {
            if (await CheckCarName(newCar.CarName))
            {
                var NewCar = _mapper.Map<Car>(newCar);
                await _unitOfWork.Repository<Car>().CreateAsync(NewCar);
                _unitOfWork.Commit();
            }
            return true;
        }

        public async Task<bool> DeleteCar(int carID)
        {
            var carFind = _unitOfWork.Repository<Car>().Where(x => x.CarId == carID).FirstOrDefault();
            if (carFind is null)
            {
                throw new CrudException<String>(System.Net.HttpStatusCode.NotFound, "Not Found this Car", "");
            }
            carFind.Status = CarState.Ban.ToString();
            await _unitOfWork.Repository<Car>().Update(carFind, carID);
            _unitOfWork.Commit();
            return true;
        }

        public async Task<IQueryable<Car>> GetCar() => _unitOfWork.Repository<Car>().GetAll().AsQueryable();


        public async Task<bool> UpdateCar(UpdateCarRequest car, int carID)
        {
            var carFind = _unitOfWork.Repository<Car>().Where(x => x.CarId == carID).FirstOrDefault();


            if (carFind is null)
            {
                throw new CrudException<String>(System.Net.HttpStatusCode.NotFound, "Not Found this Car", "");
            }
            if (car.CarName is not null)
            {
                if (await CheckCarName(car.CarName))
                {
                    carFind.CarName = car.CarName;
                }
            }
            if (car.CarType is not null)
            {
                carFind.CarType = car.CarType;
            }
            if (car.Image is not null)
            {
                carFind.Image = car.Image;
            }

            if (car.Status is not null)
            {
                carFind.Status = car.Status;
            }

            await _unitOfWork.Repository<Car>().Update(carFind, carID);
            _unitOfWork.Commit();
            return true;
        }


        private async Task<bool> CheckCarName(string CarName)
        {
            Car carName = _unitOfWork.Repository<Car>().Where(x => x.CarName == CarName).FirstOrDefault();
            if (carName is not null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "This Car Have already had in system");
            }
            return true;

        }

    }
}
