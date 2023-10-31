using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.Support.HandleError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverLicenseExamLearning_Service.ServiceBase.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BookingResponse> CreateBooking(BookingRequest req) //Teaching - Done
        {
            #region Check and Set Status Car
            int? car = req.CarId;
            var checkActiveCar = await _unitOfWork.Repository<Car>().FindAsync(x => x.Status == "Active" && x.CarId == req.CarId);
            if (checkActiveCar == null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "This car is not available right now!");
            }
            checkActiveCar.Status = "Inactive";
            _unitOfWork.Repository<Car>().Update(checkActiveCar, car.Value);
            #endregion
            #region Check and Set Mentor Status
            int? mentorId = req.MentorId;
            var checkAvailableMentor = await _unitOfWork.Repository<User>().FindAsync(x => x.Status == "Available" && x.UserId == req.MentorId && x.RoleId == 3); // 
            if (checkAvailableMentor == null)
            {
                throw new HttpStatusCodeException(System.Net.HttpStatusCode.BadRequest, "This mentor is not available right now!");
            }
            checkAvailableMentor.Status = "Not Available";
            _unitOfWork.Repository<User>().Update(checkAvailableMentor, mentorId.Value);
            #endregion
            #region V1
            //var newBooking = _mapper.Map<Booking>(req);
            //await _unitOfWork.Repository<Booking>().CreateAsync(newBooking);
            //await _unitOfWork.CommitAsync();

            //int bookingId = newBooking.BookingId;
            //var book = await GetInformationByBookId(bookingId);

            //if (book != null && book.Package != null)
            //{
            //    var package = book.Package.FirstOrDefault(x => x.PackageId == req.PackageId);
            //    var packageTypeId = package.PackageTypeId; //1 = Km, 2 = Days

            //    string type = "";
            //    int? total = 0;

            //    if (package != null)
            //    {
            //        if (packageTypeId == 1)
            //        {
            //            type = "Km";
            //            total = package.NumberOfKmOrDays;
            //        }
            //        else if (packageTypeId == 2)
            //        {
            //            type = "Days";
            //            total = package.NumberOfKmOrDays;
            //        }
            //    }

            //    var tracking = new TrackingRequest
            //    {
            //        BookingId = newBooking.BookingId,
            //        Note = "",
            //        Processing = 0,
            //        Total = total,
            //        Type = type,
            //    };

            //    var newTracking = _mapper.Map<Tracking>(tracking);
            //    await _unitOfWork.Repository<Tracking>().CreateAsync(newTracking);
            //}
            //await _unitOfWork.CommitAsync();
            #endregion 

            var newBooking = _mapper.Map<Booking>(req);
            await _unitOfWork.Repository<Booking>().CreateAsync(newBooking);
            await _unitOfWork.CommitAsync();

            var book = await GetInformationByBookId(newBooking.BookingId);
            if (book != null && book.Package != null)
            {
                var package = book.Package.FirstOrDefault(x => x.PackageId == req.PackageId);
                var packageTypeId = package.PackageTypeId;

                string type = "";
                int? total = 0;

                if (package != null)
                {
                    if (packageTypeId == 1)
                    {
                        type = "Km";
                        total = package.NumberOfKmOrDays;
                    }
                    else if (packageTypeId == 2)
                    {
                        type = "Days";
                        total = package.NumberOfKmOrDays;
                    }
                }

                var tracking = new TrackingRequest
                {
                    BookingId = newBooking.BookingId,
                    Note = "",
                    Processing = 0,
                    Total = total,
                    Type = type,
                };

                var newTracking = _mapper.Map<Tracking>(tracking);
                await _unitOfWork.Repository<Tracking>().CreateAsync(newTracking);
                _unitOfWork.Commit();
            }

            var bookingResponse = _mapper.Map<BookingResponse>(book);
            return bookingResponse;
        }

        private async Task<BookingResponse> GetInformationByBookId(int id)
        {
            var bookingResponse = await _unitOfWork.Repository<Booking>()
                .Include(b => b.Car)
                .Include(b => b.Member)
                .Include(b => b.Mentor)
                .Include(b => b.Package)
                .ThenInclude(p => p.LicenseType)
                .Where(b => b.BookingId == id)
                .Select(b => new BookingResponse
                {
                    BookingId = b.BookingId,
                    Car = new List<CarResponse>
                    {
                        new CarResponse
                        {
                            CarId = b.CarId,
                            CarName = b.Car.CarName,
                            CarType = b.Car.CarType,
                            Image = b.Car.Image,
                            Status = b.Car.Status,
                        }
                    },
                    Member = new List<UserResponse>
                    {
                        new UserResponse
                        {
                            UserName = b.Member.UserName,
                            Password = b.Member.Password,
                            RoleId = b.Member.RoleId,
                            Name = b.Member.Name,
                            PhoneNumber = b.Member.PhoneNumber,
                            Email = b.Member.Email,
                            Address = b.Member.Address,
                            Status = b.Member.Status
                        }
                    },
                    Mentor = new List<UserResponse>
                    {
                        new UserResponse
                        {
                            UserName = b.Mentor.UserName,
                            Password = b.Mentor.Password,
                            RoleId = b.Mentor.RoleId,
                            Name = b.Mentor.Name,
                            PhoneNumber = b.Mentor.PhoneNumber,
                            Email = b.Mentor.Email,
                            Address = b.Mentor.Address,
                            Status = b.Mentor.Status
                        }
                    },
                    Package = new List<PackageResponse>
                    {
                        new PackageResponse
                        {
                            PackageId = b.PackageId,
                            PackageName = b.Package.PackageName,
                            PackageTypeId = b.Package.PackageTypeId,
                            Price = b.Package.Price,
                            Description = b.Package.Description,
                            CreateDate = b.Package.CreateDate,
                            Status = b.Package.Status,
                            LicenseType = new List<LicenseTypeResponse>
                            {
                                new LicenseTypeResponse
                                {
                                    LicenseName = b.Package.LicenseType.LicenseName,
                                }
                            },
                            PackageTypes = new List<PackageTypeResponse>
                            {
                                new PackageTypeResponse
                                {
                                    PackageTypeName = b.Package.PackageType.PackageTypeName,
                                }
                            },
                            NumberOfKmOrDays = b.Package.NumberOfKmOrDays
                        }
                    },
                    CreateDate = b.CreateDate,
                    Status = b.Status
                })
                .FirstOrDefaultAsync();
            return bookingResponse;
        }

        public async Task<IEnumerable<BookingResponse>> GetAllBooking()
        {
            var bookingResponses = await _unitOfWork.Repository<Booking>()
                .Include(b => b.Car)
                .Include(b => b.Member)
                .Include(b => b.Mentor)
                .Include(b => b.Package)
                .ThenInclude(p => p.LicenseType)
                .Select(b => new BookingResponse
                {
                    BookingId = b.BookingId,
                    Car = new List<CarResponse>
                    {
                        new CarResponse
                        {
                            CarName = b.Car.CarName,
                            CarType = b.Car.CarType,
                            Image = b.Car.Image,
                            Status = b.Car.Status,
                        }
                    },
                    Member = new List<UserResponse>
                    {
                        new UserResponse
                        {
                            UserName = b.Member.UserName,
                            Password = b.Member.Password,
                            RoleId = b.Member.RoleId,
                            Name = b.Member.Name,
                            PhoneNumber = b.Member.PhoneNumber,
                            Email = b.Member.Email,
                            Address = b.Member.Address,
                            Status = b.Member.Status
                        }
                    },
                    Mentor = new List<UserResponse>
                    {
                        new UserResponse
                        {
                            UserName = b.Mentor.UserName,
                            Password = b.Mentor.Password,
                            RoleId = b.Mentor.RoleId,
                            Name = b.Mentor.Name,
                            PhoneNumber = b.Mentor.PhoneNumber,
                            Email = b.Mentor.Email,
                            Address = b.Mentor.Address,
                            Status = b.Mentor.Status
                        }
                    },
                    Package = new List<PackageResponse>
                    {
                        new PackageResponse
                        {
                            PackageId = b.Package.PackageId,
                            PackageName = b.Package.PackageName,
                            PackageTypeId = b.Package.PackageTypeId,
                            NumberOfKmOrDays = b.Package.NumberOfKmOrDays,
                            Price = b.Package.Price,
                            Description = b.Package.Description,
                            CreateDate = b.Package.CreateDate,
                            Status = b.Package.Status,
                            LicenseType = new List<LicenseTypeResponse>
                            {
                                new LicenseTypeResponse
                                {
                                    LicenseName = b.Package.LicenseType.LicenseName,
                                }
                            },
                            PackageTypes = new List<PackageTypeResponse>
                            {
                                new PackageTypeResponse
                                {
                                    PackageTypeName = b.Package.PackageType.PackageTypeName,
                                    Status = b.Package.PackageType.Status
                                }
                            }
                        }
                    },
                    CreateDate = b.CreateDate,
                    Status = b.Status
                })
                .ToListAsync();

            return bookingResponses;
        }
        public async Task<BookingResponse> UpdateBooking(int bookingId, BookingRequest req)
        {
            Booking booking = _unitOfWork.Repository<Booking>().Find(u => u.PackageId == bookingId);
            _mapper.Map<BookingRequest, Booking>(req, booking);
            await _unitOfWork.Repository<Booking>().Update(booking, bookingId);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<Booking, BookingResponse>(booking);
        }
    }
}
