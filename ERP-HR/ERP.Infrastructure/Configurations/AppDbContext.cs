using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Configurations;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Position> Positions => Set<Position>();
    public DbSet<User> Users => Set<User>();
    public DbSet<EmployeePosition> EmployeePositions => Set<EmployeePosition>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // กำหนด mapping ของ Employee ลง table ตรงนี้เลย ผ่าน modelBuilder
        // บอก EF ว่า: ชื่อ table, ความยาว column, คอลัมน์บังคับ, index ที่ unique
        // และเก็บ enum เป็น "ข้อความ" แทนตัวเลข (อ่านง่ายใน DB)
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstNameEn).HasMaxLength(100);
            entity.Property(e => e.LastNameEn).HasMaxLength(100);
            entity.Property(e => e.NationalId).IsRequired().HasMaxLength(13);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.ProfileImageUrl).HasMaxLength(500);

            // เก็บ enum เป็น string เช่น "Active", "Male"
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.Gender).HasConversion<string>().HasMaxLength(10);

            // ค่าที่ต้องไม่ซ้ำในระบบ
            entity.HasIndex(e => e.EmployeeCode).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.NationalId).IsUnique();
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("departments");

            entity.HasKey(d => d.Id);

            entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(150);
            entity.Property(d => d.NameEn).HasMaxLength(150);

            // รหัสแผนกต้องไม่ซ้ำในระบบ
            entity.HasIndex(d => d.Code).IsUnique();
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("branches");

            entity.HasKey(b => b.Id);

            entity.Property(b => b.Code).IsRequired().HasMaxLength(20);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(150);
            entity.Property(b => b.Address).HasMaxLength(500);
            entity.Property(b => b.Phone).HasMaxLength(20);

            // รหัสสาขาต้องไม่ซ้ำในระบบ
            entity.HasIndex(b => b.Code).IsUnique();
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("positions");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Code).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(150);
            entity.Property(p => p.NameEn).HasMaxLength(150);

            // เก็บ enum ระดับตำแหน่งเป็น string เช่น "Senior"
            entity.Property(p => p.Level).HasConversion<string>().HasMaxLength(20);

            // รหัสตำแหน่งต้องไม่ซ้ำในระบบ
            entity.HasIndex(p => p.Code).IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.EmployeeId).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(150);
            entity.Property(p => p.HashedPassword).IsRequired().HasMaxLength(150);

            // เก็บ enum ระดับตำแหน่งเป็น string เช่น "Senior"
            entity.Property(p => p.Role).HasConversion<string>().HasMaxLength(20);

            // รหัสตำแหน่งต้องไม่ซ้ำในระบบ
            entity.HasIndex(p => p.EmployeeId).IsUnique();
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.ToTable("employee_positions");

            entity.HasKey(ep => ep.Id);

            entity.Property(ep => ep.Salary).HasPrecision(18, 2);
            entity.Property(ep => ep.Remark).HasMaxLength(500);

            // index ปกติสำหรับ query timeline ของพนักงาน
            entity.HasIndex(ep => ep.EmployeeId, "ix_employee_positions_employee");

            // บังคับระดับ DB: พนักงาน 1 คนมีตำแหน่งปัจจุบัน (EndDate = null) ได้ทีละ 1
            // ใช้ partial unique index ของ PostgreSQL
            entity.HasIndex(ep => ep.EmployeeId, "ux_employee_positions_current")
                .IsUnique()
                .HasFilter("\"EndDate\" IS NULL");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.ToTable("holidays");

            entity.HasKey(h => h.Id);

            entity.Property(h => h.Name).IsRequired().HasMaxLength(150);

            // ห้ามมีวันหยุดซ้ำในวันเดียวกัน
            entity.HasIndex(h => new { h.Year, h.Date }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
