using Microsoft.EntityFrameworkCore;
using MiniLMS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Đăng ký kết nối SQL Server qua MiniLmsContext
builder.Services.AddDbContext<MiniLmsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình CORS để giao diện web gọi API nội bộ không bị chặn
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Cấu hình Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 1. Cho phép tự động tìm và chạy file index.html làm trang chủ
app.UseDefaultFiles();

// 2. Cho phép máy chủ đọc thư mục giao diện tĩnh wwwroot (HTML, CSS, JS, ảnh)
app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();