using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Thêm các dịch vụ vào ứng dụng
builder.AddServicesExtension();

// Kết nối DB
string connectionString = builder.Configuration.GetConnectionString("MyBlogContext");
services.ConfigureToConnectDBExtension(connectionString);

// Cấu hình để sử dụng thư viên Identity
services.ConfigureIdentityExtension();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
