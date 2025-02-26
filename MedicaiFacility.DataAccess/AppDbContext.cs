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

    public virtual DbSet<MedicalExpert> MedicalExperts { get; set; }

    public virtual DbSet<MedicalFacility> MedicalFacilities { get; set; }

    public virtual DbSet<MedicalHistory> MedicalHistories { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<RatingsAndFeedback> RatingsAndFeedbacks { get; set; }

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
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2BB4C739B");

            entity.HasIndex(e => e.AppointmentDate, "IX_Appointments_AppointmentDate");

            entity.HasIndex(e => e.ExpertId, "IX_Appointments_ExpertID");

            entity.HasIndex(e => e.FacilityId, "IX_Appointments_FacilityID");

            entity.HasIndex(e => e.PatientId, "IX_Appointments_PatientID");

            entity.HasIndex(e => e.Status, "IX_Appointments_Status");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpertId).HasColumnName("ExpertID");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Expert).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ExpertId)
                .HasConstraintName("FK__Appointme__Exper__52593CB8");

            entity.HasOne(d => d.Facility).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__Appointme__Facil__534D60F1");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__5165187F");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PK__Conversa__C050D897F8E2BDF8");

            entity.HasIndex(e => e.AdminId, "IX_Conversations_AdminID");

            entity.HasIndex(e => e.PatientId, "IX_Conversations_PatientID");

            entity.Property(e => e.ConversationId).HasColumnName("ConversationID");
            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");

            entity.HasOne(d => d.Admin).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Conversat__Admin__6FE99F9F");

            entity.HasOne(d => d.Patient).WithMany(p => p.Conversations)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Conversat__Patie__6EF57B66");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__B2079BCD82C78881");

            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.DiseaseId).HasName("PK__Diseases__69B533A962EACBF7");

            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.DiseaseName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.Diseases)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Diseases__Depart__4E88ABD4");
        });

        modelBuilder.Entity<FacilityDepartment>(entity =>
        {
            entity.HasKey(e => e.FacilityDepartmentId).HasName("PK__Facility__A8D5DCC8348D77AC");

            entity.Property(e => e.FacilityDepartmentId).HasColumnName("FacilityDepartmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Department).WithMany(p => p.FacilityDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__FacilityD__Depar__4AB81AF0");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityDepartments)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__FacilityD__Facil__49C3F6B7");
        });

        modelBuilder.Entity<HealthArticle>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__HealthAr__9C6270C8468E68C3");

            entity.HasIndex(e => e.AuthorId, "IX_HealthArticles_AuthorID");

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
                .HasConstraintName("FK__HealthArt__Autho__6B24EA82");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78C942A6022B");

            entity.HasIndex(e => e.DiseaseId, "IX_HealthRecords_DiseaseID");

            entity.HasIndex(e => e.PatientId, "IX_HealthRecords_PatientID");

            entity.HasIndex(e => e.UploadedBy, "IX_HealthRecords_UploadedBy");

            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.FilePath).IsRequired();
            entity.Property(e => e.PatientId).HasColumnName("PatientID");

            entity.HasOne(d => d.Disease).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.DiseaseId)
                .HasConstraintName("FK__HealthRec__Disea__68487DD7");

            entity.HasOne(d => d.Patient).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__HealthRec__Patie__656C112C");

            entity.HasOne(d => d.UploadedByNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.UploadedBy)
                .HasConstraintName("FK__HealthRec__Uploa__66603565");
        });

        modelBuilder.Entity<MedicalExpert>(entity =>
        {
            entity.HasKey(e => e.ExpertId).HasName("PK__MedicalE__7EDB3A384F1C6E31");

            entity.HasIndex(e => e.FacilityId, "IX_MedicalExperts_FacilityID");

            entity.HasIndex(e => e.Specialization, "IX_MedicalExperts_Specialization");

            entity.HasIndex(e => e.UserId, "IX_MedicalExperts_UserID");

            entity.Property(e => e.ExpertId).HasColumnName("ExpertID");
            entity.Property(e => e.AvailableHours).HasMaxLength(255);
            entity.Property(e => e.FacilityId).HasColumnName("FacilityID");
            entity.Property(e => e.PriceBooking).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Specialization)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Facility).WithMany(p => p.MedicalExperts)
                .HasForeignKey(d => d.FacilityId)
                .HasConstraintName("FK__MedicalEx__Facil__44FF419A");

            entity.HasOne(d => d.User).WithMany(p => p.MedicalExperts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__MedicalEx__UserI__4316F928");
        });

        modelBuilder.Entity<MedicalFacility>(entity =>
        {
            entity.HasKey(e => e.FacilityId).HasName("PK__MedicalF__5FB08B94B73264FD");

            entity.HasIndex(e => e.FacilityName, "IX_MedicalFacilities_FacilityName");

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
        });

        modelBuilder.Entity<MedicalHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__MedicalH__4D7B4ADD5186BBD1");

            entity.ToTable("MedicalHistory");

            entity.HasIndex(e => e.AppointmentId, "IX_MedicalHistory_AppointmentID");

            entity.HasIndex(e => e.Status, "IX_MedicalHistory_Status");

            entity.HasIndex(e => e.AppointmentId, "UQ__MedicalH__8ECDFCA35E467CC8").IsUnique();

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Diagnosis).IsRequired();
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Pending')");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Appointment).WithOne(p => p.MedicalHistory)
                .HasForeignKey<MedicalHistory>(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MedicalHi__Appoi__797309D9");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C037C157549E3");

            entity.HasIndex(e => e.ConversationId, "IX_Messages_ConversationID");

            entity.HasIndex(e => e.SenderId, "IX_Messages_SenderID");

            entity.Property(e => e.MessageId).HasColumnName("MessageID");
            entity.Property(e => e.ConversationId).HasColumnName("ConversationID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Messages__Conver__73BA3083");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Messages__Sender__74AE54BC");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC34666729F3F");

            entity.HasIndex(e => e.UserId, "IX_Patients_UserID");

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Patients)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Patients__UserID__3D5E1FD2");
        });

        modelBuilder.Entity<RatingsAndFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__RatingsA__6A4BEDF643A8B9E8");

            entity.ToTable("RatingsAndFeedback");

            entity.HasIndex(e => e.AppointmentId, "IX_RatingsAndFeedback_AppointmentID");

            entity.HasIndex(e => e.Rating, "IX_RatingsAndFeedback_Rating");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Feedback).HasMaxLength(100);

            entity.HasOne(d => d.Appointment).WithMany(p => p.RatingsAndFeedbacks)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__RatingsAn__Appoi__60A75C0F");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BA3BBC2E0");

            entity.HasIndex(e => e.AppointmentId, "IX_Transactions_AppointmentID");

            entity.HasIndex(e => e.PatientId, "IX_Transactions_PatientID");

            entity.HasIndex(e => e.TransactionStatus, "IX_Transactions_TransactionStatus");

            entity.HasIndex(e => e.VnPayTransactionId, "UQ__Transact__9E61BE22E4F21816").IsUnique();

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TransactionStatus)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.VnPayTransactionId)
                .HasMaxLength(255)
                .HasColumnName("VnPayTransactionID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Transacti__Appoi__59FA5E80");

            entity.HasOne(d => d.Patient).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Transacti__Patie__5AEE82B9");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACC3F22072");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.PhoneNumber, "IX_Users_PhoneNumber");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Users__85FB4E38EB11E726").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534C7E97E79").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
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
            entity.Property(e => e.Status).HasColumnName("status");
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