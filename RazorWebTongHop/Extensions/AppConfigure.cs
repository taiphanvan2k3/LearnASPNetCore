using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;
using RazorWebTongHop.Services;

namespace RazorWebTongHop.Extensions
{
    public static class AppConfigure
    {
        public static void AddServicesExtension(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            // option services
            services.AddOptions();
            var mailSettings = builder.Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);
            services.AddSingleton<IEmailSender, SendMailService>();

            services.AddRazorPages();
        }

        public static void ConfigureToConnectDBExtension(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
        }

        public static void ConfigureIdentityExtension(this IServiceCollection services)
        {
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
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
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

                // Xác định xem có yêu cầu người dùng xác thực email khi đăng kí rồi
                // mới cho đăng nhập hay không
                options.SignIn.RequireConfirmedAccount = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                /* Hoặc chỉ đường dẫn đến các file cshtml 
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; 
                */

                // Hoặc chỉ ra url đến các trang razor
                options.LoginPath = "/login";
                options.LogoutPath = "/logout";
                options.AccessDeniedPath = "/khongduoctruycap.html";
            });
        }
    }
}