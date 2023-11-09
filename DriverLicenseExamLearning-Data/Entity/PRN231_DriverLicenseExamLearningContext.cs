using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DriverLicenseExamLearning_Data.Entity
{
    public partial class PRN231_DriverLicenseExamLearningContext : DbContext
    {
        public PRN231_DriverLicenseExamLearningContext()
        {
        }

        public PRN231_DriverLicenseExamLearningContext(DbContextOptions<PRN231_DriverLicenseExamLearningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual DbSet<ExamResult> ExamResults { get; set; } = null!;
        public virtual DbSet<ExamResultDetail> ExamResultDetails { get; set; } = null!;
        public virtual DbSet<LicenseApplication> LicenseApplications { get; set; } = null!;
        public virtual DbSet<LicenseType> LicenseTypes { get; set; } = null!;
        public virtual DbSet<MemberDayRegister> MemberDayRegisters { get; set; } = null!;
        public virtual DbSet<MentorAttribute> MentorAttributes { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<PackageType> PackageTypes { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Tracking> Trackings { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings"];
            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.CarId)
                    .HasConstraintName("FK__Booking__CarId__5BE2A6F2");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.BookingMembers)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Booking__MemberI__59063A47");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.BookingMentors)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK__Booking__MentorI__5AEE82B9");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__Booking__Package__59FA5E80");
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.CarName).HasMaxLength(100);

                entity.Property(e => e.CarType).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("Exam");

                entity.Property(e => e.ExamDate).HasColumnType("date");

                entity.Property(e => e.ExamName).HasMaxLength(100);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.License)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.LicenseId)
                    .HasConstraintName("FK__Exam__LicenseId__4316F928");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasKey(e => e.ExamQuestions)
                    .HasName("PK__ExamQues__F79CD654EE271CE7");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__ExamQuest__ExamI__5070F446");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__ExamQuest__Quest__5165187F");
            });

            modelBuilder.Entity<ExamResult>(entity =>
            {
                entity.ToTable("ExamResult");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Result).HasMaxLength(20);

                entity.HasOne(d => d.Exam)
                    .WithMany(p => p.ExamResults)
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__ExamResul__ExamI__45F365D3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ExamResults)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__ExamResul__UserI__46E78A0C");
            });

            modelBuilder.Entity<ExamResultDetail>(entity =>
            {
                entity.HasKey(e => e.ExamResultDetailsId)
                    .HasName("PK__ExamResu__9F70203F76EE069F");

                entity.Property(e => e.WrongAnswer).HasMaxLength(2000);

                entity.HasOne(d => d.ExamResult)
                    .WithMany(p => p.ExamResultDetails)
                    .HasForeignKey(d => d.ExamResultId)
                    .HasConstraintName("FK__ExamResul__ExamR__4CA06362");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ExamResultDetails)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__ExamResul__Quest__4D94879B");
            });

            modelBuilder.Entity<LicenseApplication>(entity =>
            {
                entity.ToTable("LicenseApplication");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.LicenseApplications)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK__LicenseAp__Licen__693CA210");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LicenseApplications)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__LicenseAp__UserI__68487DD7");
            });

            modelBuilder.Entity<LicenseType>(entity =>
            {
                entity.ToTable("LicenseType");

                entity.Property(e => e.LicenseName).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<MemberDayRegister>(entity =>
            {
                entity.ToTable("MemberDayRegister");

                entity.Property(e => e.Datetime).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.MemberDayRegisters)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__MemberDay__Booki__619B8048");
            });

            modelBuilder.Entity<MentorAttribute>(entity =>
            {
                entity.ToTable("MentorAttribute");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MentorAttributes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__MentorAtt__UserI__3E52440B");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.ToTable("Package");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.LicenseTypeId).HasColumnName("LicenseTypeID");

                entity.Property(e => e.PackageName).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("int");

                entity.Property(e => e.NumberOfKmOrDays).HasColumnType("int");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK_Package_LicenseType");

                entity.HasOne(d => d.PackageType)
                    .WithMany(p => p.Packages)
                    .HasForeignKey(d => d.PackageTypeId)
                    .HasConstraintName("FK__Package__Package__5629CD9C");
            });

            modelBuilder.Entity<PackageType>(entity =>
            {
                entity.ToTable("PackageType");

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Answer).HasMaxLength(255);

                entity.Property(e => e.Option1).HasMaxLength(255);

                entity.Property(e => e.Option2).HasMaxLength(255);

                entity.Property(e => e.Option3).HasMaxLength(255);

                entity.Property(e => e.Option4).HasMaxLength(255);

                entity.Property(e => e.Question1).HasColumnName("Question");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.HasOne(d => d.LicenseTypeNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.LicenseType)
                    .HasConstraintName("FK__Question__Licens__49C3F6B7");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<Tracking>(entity =>
            {
                entity.ToTable("Tracking");

                entity.Property(e => e.Processing).HasMaxLength(50);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Type).HasMaxLength(20);

                entity.Property(e => e.Processing).HasColumnType("int");

                entity.Property(e => e.Total).HasColumnType("int");

                entity.Property(e => e.TrackingDate).HasColumnType("date");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Trackings)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Tracking__Bookin__5EBF139D");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.TransactionType).HasMaxLength(20);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.Total).HasColumnType("int");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Transacti__Booki__6477ECF3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Transacti__UserI__656C112C");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.RefreshToken).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(20);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__RoleId__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
