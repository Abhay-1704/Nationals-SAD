using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Session2_2019.Models;

public partial class Session2Context : DbContext
{
    public Session2Context()
    {
    }

    public Session2Context(DbContextOptions<Session2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetGroup> AssetGroups { get; set; }

    public virtual DbSet<ChangedPart> ChangedParts { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepartmentLocation> DepartmentLocations { get; set; }

    public virtual DbSet<EmergencyMaintenance> EmergencyMaintenances { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Session2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asset>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssetGroupId).HasColumnName("AssetGroupID");
            entity.Property(e => e.AssetName).HasMaxLength(150);
            entity.Property(e => e.AssetSn)
                .HasMaxLength(20)
                .HasColumnName("AssetSN");
            entity.Property(e => e.DepartmentLocationId).HasColumnName("DepartmentLocationID");
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

            entity.HasOne(d => d.AssetGroup).WithMany(p => p.Assets)
                .HasForeignKey(d => d.AssetGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assets_AssetGroups");

            entity.HasOne(d => d.DepartmentLocation).WithMany(p => p.Assets)
                .HasForeignKey(d => d.DepartmentLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assets_DepartmentLocations");

            entity.HasOne(d => d.Employee).WithMany(p => p.Assets)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Assets_Employees");
        });

        modelBuilder.Entity<AssetGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssetTypes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ChangedPart>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmergencyMaintenanceId).HasColumnName("EmergencyMaintenanceID");
            entity.Property(e => e.PartId).HasColumnName("PartID");

            entity.HasOne(d => d.EmergencyMaintenance).WithMany(p => p.ChangedParts)
                .HasForeignKey(d => d.EmergencyMaintenanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChangedParts_EmergencyMaintenances");

            entity.HasOne(d => d.Part).WithMany(p => p.ChangedParts)
                .HasForeignKey(d => d.PartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ChangedParts_Parts");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<DepartmentLocation>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");

            entity.HasOne(d => d.Department).WithMany(p => p.DepartmentLocations)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentLocations_Departments");

            entity.HasOne(d => d.Location).WithMany(p => p.DepartmentLocations)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DepartmentLocations_Locations");
        });

        modelBuilder.Entity<EmergencyMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_EMS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssetId).HasColumnName("AssetID");
            entity.Property(e => e.DescriptionEmergency).HasMaxLength(200);
            entity.Property(e => e.EmendDate).HasColumnName("EMEndDate");
            entity.Property(e => e.EmreportDate).HasColumnName("EMReportDate");
            entity.Property(e => e.EmstartDate).HasColumnName("EMStartDate");
            entity.Property(e => e.EmtechnicianNote)
                .HasMaxLength(200)
                .HasColumnName("EMTechnicianNote");
            entity.Property(e => e.OtherConsiderations).HasMaxLength(200);
            entity.Property(e => e.PriorityId).HasColumnName("PriorityID");

            entity.HasOne(d => d.Asset).WithMany(p => p.EmergencyMaintenances)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmergencyMaintenances_Assets");

            entity.HasOne(d => d.Priority).WithMany(p => p.EmergencyMaintenances)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmergencyMaintenances_Priorities");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
