using ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Configurations;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

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

        base.OnModelCreating(modelBuilder);
    }
}
