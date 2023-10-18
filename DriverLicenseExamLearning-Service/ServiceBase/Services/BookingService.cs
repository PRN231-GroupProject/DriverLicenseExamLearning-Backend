using AutoMapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Request;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using Twilio.TwiML;

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
        public async Task<bool> CreateBooking(BookingRequest req)
        {
            var newBooking = _mapper.Map<Booking>(req);
            await _unitOfWork.Repository<Booking>().CreateAsync(newBooking);
            await _unitOfWork.CommitAsync();

            int bookingId = newBooking.BookingId;
            var book = await GetInformationByBookId(bookingId);

            if (book != null && book.Package != null)
            {
                var packageTypeId = book.Package.FirstOrDefault(row => true)?.PackageTypeId;
                string processing = "0";
                string total = "";

                if (packageTypeId == 1)
                {
                    total = "800km";
                }
                if (packageTypeId == 2)
                {
                    total = "15 days";
                }
                if (packageTypeId == 3)
                {
                    total = "30 days";
                }
                if (packageTypeId == 4)
                {
                    total = "1600km";
                }

                var tracking = new TrackingRequest
                {
                    BookingId = newBooking.BookingId,
                    Note = "",
                    Processing = processing,
                    Total = total
                };

                var newTracking = _mapper.Map<Tracking>(tracking);
                await _unitOfWork.Repository<Tracking>().CreateAsync(newTracking);
            }

            await _unitOfWork.CommitAsync();
            return true;
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
                            PackageName = b.Package.PackageName,
                            PackageTypeId = b.PackageId,
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
                            PackageName = b.Package.PackageName,
                            PackageTypeId = b.Package.PackageId,
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
