using ERP.Api;
using ERP.Application.Interfaces;
using ERP.Application.Services;
using ERP.Application.Validators;
using ERP.Infrastructure.Configurations;
using ERP.Infrastructure.Repositories;
using ERP.Infrastructure.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ---- ลงทะเบียน service เข้า DI container ----

// 1) Database (EF Core + PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2) Repository + Service (Scoped = หนึ่ง instance ต่อหนึ่ง request)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeePositionRepository, EmployeePositionRepository>();
builder.Services.AddScoped<IEmployeePositionService, EmployeePositionService>();
builder.Services.AddScoped<IHolidayRepository, HolidayRepository>();
builder.Services.AddScoped<IHolidayService, HolidayService>();
builder.Services.AddScoped<ILeaveAllocationRepository, LeaveAllocationRepository>();
builder.Services.AddScoped<ILeaveAllocationService, LeaveAllocationService>();

// hashing รหัสผ่าน — stateless ใช้ Singleton ได้
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// 3) FluentValidation — สแกนหา validator ทุกตัวใน assembly เดียวกับ CreateEmployeeRequestValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeRequestValidator>();

// 4) ตัวจัดการ error รวมศูนย์ → ProblemDetails
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 5) Controllers + OpenAPI document
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        // ให้ JSON รับ/ส่ง enum เป็นชื่อข้อความ เช่น "Male" แทนเลข 0
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddOpenApi();

var app = builder.Build();

// ---- HTTP pipeline ----
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // เปิดดู spec ได้ที่ /openapi/v1.json
}

app.MapControllers();

app.Run();
