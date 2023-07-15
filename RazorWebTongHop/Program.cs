using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;
using RazorWebTongHop.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddOptions();
// Lấy section ra
var mailSettings = builder.Configuration.GetSection("MailSettings");
services.Configure<MailSettings>(mailSettings);
services.AddSingleton<IEmailSender, SendMailService>();

builder.Services.AddRazorPages();
string connectionString = builder.Configuration.GetConnectionString("MyBlogContext");

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));


// Đăng kí Identity
services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

// Sử dụng trang mặc định
// services.AddDefaultIdentity<AppUser>()
//         .AddEntityFrameworkStores<DataContext>()
//         .AddDefaultTokenProviders();

// Truy cập IdentityOptions
services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    // các ký tự đặt tên user
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    // Email là duy nhất, nếu user nào đó đã dùng email này rồi thì user khác không
    // thể dùng lại email này nữa.
    options.User.RequireUniqueEmail = true;

    // Cấu hình đăng nhập.
    // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedEmail = true;
    // Xác thực số điện thoại   
    options.SignIn.RequireConfirmedPhoneNumber = false;

});

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
