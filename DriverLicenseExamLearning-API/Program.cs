using DriverLicenseExamLearning_API;
using DriverLicenseExamLearning_API.Mapper;
using DriverLicenseExamLearning_Data.Entity;
using DriverLicenseExamLearning_Data.UnitOfWork;
using DriverLicenseExamLearning_Service.DTOs.Response;
using DriverLicenseExamLearning_Service.ServiceBase.IServices;
using DriverLicenseExamLearning_Service.ServiceBase.Services;
using DriverLicenseExamLearning_Service.Support.HandleError;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
//modelBuilder.EntitySet<ExamGetByMemberResponse>("ExamGetByMemberResponse");
//modelBuilder.EntitySet<ExamGetByLicenseTye>("ExamGetByLicenseTye");
//modelBuilder.EntitySet<Booking>("Bookings");
//modelBuilder.EntitySet<Exam>("Exams");
//modelBuilder.EntitySet<ExamQuestion>("ExamQuestions");
//modelBuilder.EntitySet<LicenseApplication>("LicenseApplications");
//modelBuilder.EntitySet<LicenseType>("LicenseTypes");
//modelBuilder.EntitySet<MemberAttribute>("MemberAttributes");
//modelBuilder.EntitySet<MentorAttribute>("MentorAttributes");
//modelBuilder.EntitySet<MentorAvailability>("MentorAvailabilitys");
//modelBuilder.EntitySet<Package>("Packages");
//modelBuilder.EntitySet<Purchase>("Purchases");
//modelBuilder.EntitySet<Question>("Questions");
//modelBuilder.EntitySet<Role>("Roles");
//modelBuilder.EntitySet<User>("Users");

builder.Services.AddControllers()
                .AddOData(options => options.Select()
                                .Filter()
                                .OrderBy()
                                .Expand()
                                .Count()
                                .SetMaxTop(null)
                                .AddRouteComponents("odata", modelBuilder.GetEdmModel()));
// Add Dependency Injection

builder.Services.AddTransient<IMentorApplication, MentorApplication>();
builder.Services.AddScoped<ILicenseApplicationService, LicenseApplicationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IMemberDayRegisterService, MemberDayRegisterService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IClaimsService, ClaimsService>();
builder.Services.AddScoped<IPackagesService, PackageService>();
builder.Services.AddScoped<ITrackingService, TrackingService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<ILicenseTypeService, LicenseTypeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Mapping));
builder.Services.AddScoped<PRN231_DriverLicenseExamLearningContext>();
builder.Services.AddTransient<IQuestionBankService, QuestionBankService>();
// Add authentication and configure JWT

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWTSecretKey:Issuer"],
            ValidAudience = builder.Configuration["JWTSecretKey:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSecretKey:SecretKey"])),
            ClockSkew = TimeSpan.FromSeconds(1)
        };
    });
// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddSwaggerGen(opt =>
{
    opt.EnableAnnotations();
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "DriverLicenseExamLearning-API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer",
    });
    opt.OperationFilter<AuthorizeOperationFilter>();
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.DefaultModelExpandDepth(0);
    c.DefaultModelsExpandDepth(-1);
    c.DefaultModelRendering(ModelRendering.Example);
    c.DisplayOperationId();
    c.DisplayRequestDuration();
    c.DocExpansion(DocExpansion.None);
    c.EnableDeepLinking();
    c.EnableFilter();
    c.ShowExtensions();
    c.EnableValidator();
});

app.Use((context, next) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
    return next.Invoke();
});

app.UseCors();

app.UseRouting();
app.UseMiddleware<CustomExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
