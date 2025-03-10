﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using MedicaiFacility.BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MedicaiFacility.DataAccess;

public partial class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }



    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<FacilityDepartment> FacilityDepartments { get; set; }

    public virtual DbSet<HealthArticle> HealthArticles { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<HealthRecordDisease> HealthRecordDiseases { get; set; }

    public virtual DbSet<MedicalExpert> MedicalExperts { get; set; }

    public virtual DbSet<MedicalExpertSchedule> MedicalExpertSchedules { get; set; }

    public virtual DbSet<MedicalFacility> MedicalFacilities { get; set; }

    public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<RatingsAndFeedback> RatingsAndFeedbacks { get; set; }

    public virtual DbSet<SystemBalance> SystemBalances { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA20EB1E33B");

            entity.HasIndex(e => e.ExpertId, "IX_Appointments_ExpertID");

            entity.HasIndex(e => e.PatientId, "IX_Appointments_PatientID");

            entity.HasIndex(e => e.Status, "IX_Appointments_Status");

            entity.HasIndex(e => e.TransactionId, "UQ__Appointm__55433A4A09F5CA19").IsUnique();

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ExpertId).HasColumnName("ExpertID");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Expert).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ExpertId)
                .HasConstraintName("FK__Appointme__Exper__534D60F1");

            entity.HasOne(d => d.Facility).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Appointme__Facil__5441852A");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__52593CB8");

            entity.HasOne(d => d.Transaction).WithOne(p => p.Appointment)
                .HasForeignKey<Appointment>(d => d.TransactionId)
                .HasConstraintName("FK__Appointme__Trans__5629CD9C");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PK__Conversa__C050D897F14CF978");

            entity.Property(e => e.ConversationId).HasColumnName("ConversationID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("FK_Conversations_Users");

            entity.HasOne(d => d.Patient).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Conversat__Patie__71D1E811");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD4A004519");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.DiseaseId).HasName("PK__Diseases__69B533A9596C250D");

            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DiseaseName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Department).WithMany(p => p.Diseases)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Diseases__Depart__47DBAE45");
        });

        modelBuilder.Entity<FacilityDepartment>(entity =>
        {
            entity.HasKey(e => e.FacilityDepartmentId).HasName("PK__Facility__A8D5DCC848DCB91D");

            entity.Property(e => e.FacilityDepartmentId).HasColumnName("FacilityDepartmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Department).WithMany(p => p.FacilityDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__FacilityD__Depar__4222D4EF");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityDepartments)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__FacilityD__Facil__412EB0B6");
        });

        modelBuilder.Entity<HealthArticle>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__HealthAr__9C6270C80CD1F39A");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.HealthArticles)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__HealthArt__Autho__6E01572D");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78C93349F4A2");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.SharedLink).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__HealthRec__Appoi__656C112C");
        });

        modelBuilder.Entity<HealthRecordDisease>(entity =>
        {
            entity.HasKey(e => e.HealthRecordDiseaseId).HasName("PK__HealthRe__FC643971C6AFC9D8");

            entity.Property(e => e.HealthRecordDiseaseId).HasColumnName("HealthRecordDiseaseID");
            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.RecordId).HasColumnName("RecordID");

            entity.HasOne(d => d.Disease).WithMany(p => p.HealthRecordDiseases)
                .HasForeignKey(d => d.DiseaseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HealthRec__Disea__6B24EA82");

            entity.HasOne(d => d.Record).WithMany(p => p.HealthRecordDiseases)
                .HasForeignKey(d => d.RecordId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HealthRec__Recor__6A30C649");
        });

        modelBuilder.Entity<MedicalExpert>(entity =>
        {
            entity.HasKey(e => e.ExpertId).HasName("PK__MedicalE__7EDB3A384C59FE61");

            entity.Property(e => e.ExpertId)
                .ValueGeneratedNever()
                .HasColumnName("ExpertID");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.PriceBooking).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Specialization)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(d => d.Expert).WithOne(p => p.MedicalExpert)
                .HasForeignKey<MedicalExpert>(d => d.ExpertId)
                .HasConstraintName("FK__MedicalEx__Exper__3A81B327");

            entity.HasOne(d => d.Facility).WithMany(p => p.MedicalExperts)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__MedicalEx__Facil__398D8EEE");
        });

        modelBuilder.Entity<MedicalExpertSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__MedicalE__9C8A5B69DFB080A9");

            entity.ToTable("MedicalExpertSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.DayOfWeek)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.ExpertId).HasColumnName("ExpertID");

            entity.HasOne(d => d.Expert).WithMany(p => p.MedicalExpertSchedules)
                .HasForeignKey(d => d.ExpertId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MedicalEx__Exper__3D5E1FD2");
        });

        modelBuilder.Entity<MedicalFacility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__MedicalF__5FB08B94A334A299");

            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ContactInfo).HasMaxLength(255);
            entity.Property(e => e.FacilityName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FacilityType)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
            entity.Property(e => e.Verified).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<MedicalHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__MedicalH__4D7B4ADDE601959A");

            entity.ToTable("MedicalHistory");

            entity.HasIndex(e => e.AppointmentId, "UQ__MedicalH__8ECDFCA3338FA33C").IsUnique();

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithOne(p => p.MedicalHistory)
                .HasForeignKey<MedicalHistory>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MedicalHi__Appoi__7C4F7684");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C037C38C9ABD2");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.ConversationId).HasColumnName("ConversationID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Messages__Conver__76969D2E");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK_Messages_Users");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC346618234A6");

            entity.Property(e => e.PatientId)
                .ValueGeneratedNever()
                .HasColumnName("PatientID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Gender).HasMaxLength(10);

            entity.HasOne(d => d.PatientNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.PatientId)
                .HasConstraintName("FK__Patients__Patien__2E1BDC42");
        });

        modelBuilder.Entity<RatingsAndFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__RatingsA__6A4BEDF697395E0D");

            entity.ToTable("RatingsAndFeedback");

            entity.HasIndex(e => e.AppointmentId, "UQ__RatingsA__8ECDFCA377FCEB30").IsUnique();

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithOne(p => p.RatingsAndFeedback)
                .HasForeignKey<RatingsAndFeedback>(d => d.AppointmentId)
                .HasConstraintName("FK__RatingsAn__Appoi__60A75C0F");
        });

        modelBuilder.Entity<SystemBalance>(entity =>
        {
            entity.HasKey(e => e.BalanceId).HasName("PK__SystemBa__A760D59ED77A8DD4");

            entity.Property(e => e.BalanceId).HasColumnName("BalanceID");
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PendingDebt).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalBalance).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4B220D83E6");

            entity.HasIndex(e => e.TransactionStatus, "IX_Transactions_TransactionStatus");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionStatus)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TransactionType).IsRequired();
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Transacti__UserI__4AB81AF0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC15FCF153");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.UserType, "IX_Users_UserType");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__85FB4E38AB50D993").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053451EDF936").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.BankAccount).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserType)
                .IsRequired()
                .HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}