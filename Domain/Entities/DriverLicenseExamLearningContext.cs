﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace Domain.Entities
{
    public partial class DriverLicenseExamLearningContext : DbContext
    {
        public DriverLicenseExamLearningContext()
        {
        }

        public DriverLicenseExamLearningContext(DbContextOptions<DriverLicenseExamLearningContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<ExamQuestion> ExamQuestions { get; set; } = null!;
        public virtual DbSet<LicenseApplication> LicenseApplications { get; set; } = null!;
        public virtual DbSet<LicenseType> LicenseTypes { get; set; } = null!;
        public virtual DbSet<MemberAttribute> MemberAttributes { get; set; } = null!;
        public virtual DbSet<MentorAttribute> MentorAttributes { get; set; } = null!;
        public virtual DbSet<MentorAvailability> MentorAvailabilities { get; set; } = null!;
        public virtual DbSet<Package> Packages { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
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
                entity.Property(e => e.BookingId).ValueGeneratedNever();

                entity.Property(e => e.BookingDate).HasColumnType("datetime");

                entity.Property(e => e.FeePaid).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Availability)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.AvailabilityId)
                    .HasConstraintName("FK__Bookings__Availa__5629CD9C");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Bookings__Member__5441852A");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK__Bookings__Mentor__5535A963");
            });

            modelBuilder.Entity<Exam>(entity =>
            {
                entity.Property(e => e.ExamId).ValueGeneratedNever();

                entity.Property(e => e.ExamDate).HasColumnType("datetime");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.Exams)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK__Exams__LicenseTy__4AB81AF0");
            });

            modelBuilder.Entity<ExamQuestion>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Exam)
                    .WithMany()
                    .HasForeignKey(d => d.ExamId)
                    .HasConstraintName("FK__ExamQuest__ExamI__4CA06362");

                entity.HasOne(d => d.Question)
                    .WithMany()
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__ExamQuest__Quest__4D94879B");
            });

            modelBuilder.Entity<LicenseApplication>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .HasName("PK__LicenseA__C93A4C99B7DDD0AB");

                entity.Property(e => e.ApplicationId).ValueGeneratedNever();

                entity.Property(e => e.ApplicationDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.LicenseApplications)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK__LicenseAp__Licen__59FA5E80");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.LicenseApplications)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__LicenseAp__Membe__59063A47");
            });

            modelBuilder.Entity<LicenseType>(entity =>
            {
                entity.Property(e => e.LicenseTypeId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<MemberAttribute>(entity =>
            {
                entity.HasKey(e => e.MemberId)
                    .HasName("PK__MemberAt__0CF04B186E710DAE");

                entity.Property(e => e.MemberId).ValueGeneratedNever();

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.MemberAttributes)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK__MemberAtt__Licen__4222D4EF");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MemberAttributes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__MemberAtt__UserI__412EB0B6");
            });

            modelBuilder.Entity<MentorAttribute>(entity =>
            {
                entity.HasKey(e => e.MentorId)
                    .HasName("PK__MentorAt__053B7E985DF5DD6E");

                entity.Property(e => e.MentorId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MentorAttributes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__MentorAtt__UserI__3E52440B");
            });

            modelBuilder.Entity<MentorAvailability>(entity =>
            {
                entity.HasKey(e => e.AvailabilityId)
                    .HasName("PK__MentorAv__DA3979B172763390");

                entity.ToTable("MentorAvailability");

                entity.Property(e => e.AvailabilityId).ValueGeneratedNever();

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.MentorAvailabilities)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK__MentorAva__Licen__5165187F");

                entity.HasOne(d => d.Mentor)
                    .WithMany(p => p.MentorAvailabilities)
                    .HasForeignKey(d => d.MentorId)
                    .HasConstraintName("FK__MentorAva__Mento__5070F446");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.Property(e => e.PackageId).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.PurchaseId).ValueGeneratedNever();

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK__Purchases__Membe__5EBF139D");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.PackageId)
                    .HasConstraintName("FK__Purchases__Packa__5FB337D6");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).ValueGeneratedNever();

                entity.HasOne(d => d.LicenseType)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.LicenseTypeId)
                    .HasConstraintName("FK__Questions__Licen__47DBAE45");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.RefreshToken).HasMaxLength(1000);
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__RoleId__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
