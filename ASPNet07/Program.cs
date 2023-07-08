var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddRazorPages().AddRazorPagesOptions(options =>
{
    // Đổi thư mục mặc định lưu các trang razor page
    options.RootDirectory = "/Pages";

    // Viết lại các url truy cập đến razor page, mặc định là /<tên file razor>
    options.Conventions.AddPageRoute("/FirstPage", "/trang-dau-tien.html");
    options.Conventions.AddPageRoute("/SecondPage", "/MyPages/trang-thu-hai.html");
});

services.Configure<RouteOptions>(routeOptions =>
{
    // Chuyển tất cả url phát sinh từ tag helper ra thành in thường
    routeOptions.LowercaseUrls = true;
});

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    app.MapGet("/", () => "Hello World!");

    // Thiết lập endpoint map razor page
    // EndpointRoutingMiddleware sẽ tạo ra các endpoints: /FirstPage, /SecondPage, /ThirdPage
    // Nếu có thư mục con bên trong Pages thì nó tạo ra endpoints: vd /DichVu/DichVu1
    app.MapRazorPages();
});

app.Run();
