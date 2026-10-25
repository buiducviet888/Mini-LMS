using Microsoft.AspNetCore.StaticFiles;
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

// 2. Cấu hình MIME type và phục vụ file tĩnh trong wwwroot (cho phép tải file lạ như .ipynb)
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".ipynb"] = "application/json";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    ServeUnknownFileTypes = true, // Phục vụ cả các định dạng file không có trong danh sách mặc định
    DefaultContentType = "application/octet-stream"
});

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();