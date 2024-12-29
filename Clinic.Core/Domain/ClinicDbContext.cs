using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Clinic.Core.Domain;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MedicinesAssigned> MedicinesAssigneds { get; set; }

    public virtual DbSet<NotWorkingDay> NotWorkingDays { get; set; }

    public virtual DbSet<Procedure> Procedures { get; set; }

    public virtual DbSet<ProcedureImage> ProcedureImages { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<VisitStatus> VisitStatuses { get; set; }

    public virtual DbSet<VisitsProcedure> VisitsProcedures { get; set; }

    public virtual DbSet<WeekDay> WeekDays { get; set; }

    public virtual DbSet<WeekDaySchedule> WeekDaySchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=clinic_db;user=root;password=ADMIN", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<MedicinesAssigned>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("medicines_assigned");

            entity.HasIndex(e => e.DoctorId, "doctor_id");

            entity.HasIndex(e => e.PatientId, "patient_id");

            entity.HasIndex(e => e.VisitProcedureId, "visit_procedure_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DayCount).HasColumnName("day_count");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Dose)
                .HasMaxLength(50)
                .HasColumnName("dose");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.VisitProcedureId).HasColumnName("visit_procedure_id");

            entity.HasOne(d => d.Doctor).WithMany(p => p.MedicinesAssignedDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medicines_assigned_ibfk_1");

            entity.HasOne(d => d.Patient).WithMany(p => p.MedicinesAssignedPatients)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medicines_assigned_ibfk_2");

            entity.HasOne(d => d.VisitProcedure).WithMany(p => p.MedicinesAssigneds)
                .HasForeignKey(d => d.VisitProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medicines_assigned_ibfk_3");
        });

        modelBuilder.Entity<NotWorkingDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("not_working_days");

            entity.HasIndex(e => e.DoctorId, "doctor_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.NotWorkDate).HasColumnName("not_work_date");

            entity.HasOne(d => d.Doctor).WithMany(p => p.NotWorkingDays)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("not_working_days_ibfk_1");
        });

        modelBuilder.Entity<Procedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("procedures");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10)
                .HasColumnName("price");
        });

        modelBuilder.Entity<ProcedureImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("procedure_images");

            entity.HasIndex(e => e.VisitProcedureId, "visit_procedure_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Url)
                .HasMaxLength(1000)
                .HasColumnName("url");
            entity.Property(e => e.VisitProcedureId).HasColumnName("visit_procedure_id");

            entity.HasOne(d => d.VisitProcedure).WithMany(p => p.ProcedureImages)
                .HasForeignKey(d => d.VisitProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("procedure_images_ibfk_1");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("specializations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Phone, "phone").IsUnique();

            entity.HasIndex(e => e.TypesId, "types_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1000)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(1000)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.TypesId).HasColumnName("types_id");

            entity.HasOne(d => d.Types).WithMany(p => p.Users)
                .HasForeignKey(d => d.TypesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_ibfk_1");

            entity.HasMany(d => d.Specializations).WithMany(p => p.Doctors)
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorsSpecialization",
                    r => r.HasOne<Specialization>().WithMany()
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("doctors_specializations_ibfk_2"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("doctors_specializations_ibfk_1"),
                    j =>
                    {
                        j.HasKey("DoctorId", "SpecializationId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("doctors_specializations");
                        j.HasIndex(new[] { "SpecializationId" }, "specialization_id");
                        j.IndexerProperty<long>("DoctorId").HasColumnName("doctor_id");
                        j.IndexerProperty<int>("SpecializationId").HasColumnName("specialization_id");
                    });
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visits");

            entity.HasIndex(e => e.DoctorId, "doctor_id");

            entity.HasIndex(e => e.PatientId, "patient_id");

            entity.HasIndex(e => e.StatusId, "status_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EndActualDate)
                .HasColumnType("timestamp")
                .HasColumnName("end_actual_date");
            entity.Property(e => e.EndScheduledDate)
                .HasColumnType("timestamp")
                .HasColumnName("end_scheduled_date");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.StartActualDate)
                .HasColumnType("timestamp")
                .HasColumnName("start_actual_date");
            entity.Property(e => e.StartScheduledDate)
                .HasColumnType("timestamp")
                .HasColumnName("start_scheduled_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Doctor).WithMany(p => p.VisitDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visits_ibfk_1");

            entity.HasOne(d => d.Patient).WithMany(p => p.VisitPatients)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visits_ibfk_2");

            entity.HasOne(d => d.Status).WithMany(p => p.Visits)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visits_ibfk_3");
        });

        modelBuilder.Entity<VisitStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visit_statuses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VisitsProcedure>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visits_procedures");

            entity.HasIndex(e => e.ProcedureId, "procedure_id");

            entity.HasIndex(e => e.VisitId, "visit_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Notes)
                .HasMaxLength(1000)
                .HasColumnName("notes");
            entity.Property(e => e.ProcedureId).HasColumnName("procedure_id");
            entity.Property(e => e.VisitId).HasColumnName("visit_id");

            entity.HasOne(d => d.Procedure).WithMany(p => p.VisitsProcedures)
                .HasForeignKey(d => d.ProcedureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visits_procedures_ibfk_2");

            entity.HasOne(d => d.Visit).WithMany(p => p.VisitsProcedures)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("visits_procedures_ibfk_1");
        });

        modelBuilder.Entity<WeekDay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("week_day");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<WeekDaySchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("week_day_schedule");

            entity.HasIndex(e => e.DoctorId, "doctor_id");

            entity.HasIndex(e => e.WeekDayId, "week_day_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BreakEndTime)
                .HasColumnType("time")
                .HasColumnName("break_end_time");
            entity.Property(e => e.BreakStartTime)
                .HasColumnType("time")
                .HasColumnName("break_start_time");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.WeekDayId).HasColumnName("week_day_id");

            entity.HasOne(d => d.Doctor).WithMany(p => p.WeekDaySchedules)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("week_day_schedule_ibfk_1");

            entity.HasOne(d => d.WeekDay).WithMany(p => p.WeekDaySchedules)
                .HasForeignKey(d => d.WeekDayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("week_day_schedule_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
