using System.Text;
using ASPNet04.Middlewares;
using ASPNet04.Options;
using ASPNet04.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Đăng kí dịch vụ
var services = builder.Services;

// Add service dưới dạng option vào hệ thống
services.AddOptions();
var testOption = builder.Configuration.GetSection("TestOptions");
// Inject vào hệ thống option này, ở đâu muốn lấy ra
// value của section đó thì truy cập thông qua đối tượng TestOption

// Khi ta đưa option vào service thông qua thì lúc này nó thêm 1 dịch vụ
// có kiểu IOptions<TestOption>. Dùng IOptions<Testoption>
// để inject vào controller hoặc API để dùng. Nó sẽ lưu thông tin về key,value
// của section TestOptions
services.Configure<TestOption>(testOption);

// Do đây là danh sách cố định nên dùng Singleton là được rồi
services.AddSingleton<ProductNameService>();

// Add Middlware vào dưới dạng service vào hệ thống
// Do đây là ta tạo 1 Middleware bằng cách kế thừa IMiddleware nên mới phải add vào service
services.AddTransient<TestOptionMiddleware>();

// Nhắc lại: tất cả việc add service phải thực hiện trước khi builder.Build()
var app = builder.Build();

// Request phải đi qua Middleware TestOptionMiddleware trước khi xuống 
// EndpointRoutingMiddleware
app.UseMiddleware<TestOptionMiddleware>();

// Add EndpointRoutingMiddleware vào pipeline
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    // Nếu dùng endpoints.MapGet("/", () => "Trang chu"); thì sẽ bị lỗi khi Middleware trước đó WriteAsync4

    endpoints.MapGet("/", async context =>
    {
        string scheme = context.Request.Scheme;
        await context.Response.WriteAsync($"Dang su dung scheme: {scheme}\n");
        await context.Response.WriteAsync("Trang chu");
    });

    endpoints.MapGet("/ShowOptions", async context =>
    {
        // Có thể lấy service prodiver của ứng dụng
        // Lấy ra configuration, nhiệm vụ của nó là nạp các cấu hình trong appsettings
        var configuration = context.RequestServices.GetService<IConfiguration>();
        var testOption = configuration.GetSection("TestOptions");
        var opt_key1 = testOption["opt_key1"];

        var subSection = testOption.GetSection("opt_key2");
        var k1 = subSection["k1"];
        var k2 = subSection["k2"];

        string previousMessage = (context.Items["info"] as string) + (context.Items["content"] as string);
        StringBuilder sb = new StringBuilder();
        sb.Append("TestOptions\n");
        sb.Append($"opt_key1 = {opt_key1}\n");
        sb.Append($"Testoptions.opt_key2.k1 = {k1}\n");
        sb.Append($"Testoptions.opt_key2.k2 = {k2}");

        await context.Response.WriteAsync(previousMessage); 
        await context.Response.WriteAsync("Show options in /ShowOption\n");
        await context.Response.WriteAsync(sb.ToString());
    });

    endpoints.MapGet("/ShowOptions2", async context =>
    {
        var configuration = context.RequestServices.GetService<IConfiguration>();

        // Sau khi lấy ra section thì convert nó qua class luôn
        var testOption = configuration.GetSection("TestOptions").Get<TestOption>();

        StringBuilder sb = new StringBuilder();
        sb.Append("TestOptions\n");
        sb.Append($"opt_key1 = {testOption.opt_key1}\n");
        sb.Append($"Testoptions.opt_key2.k1 = {testOption.opt_key2.k1}\n");
        sb.Append($"Testoptions.opt_key2.k2 = {testOption.opt_key2.k2}");

        await context.Response.WriteAsync(sb.ToString());
    });

    endpoints.MapGet("/ShowOptions3", async context =>
    {
        var testOption = context.RequestServices.GetService<IOptions<TestOption>>()
                                                  .Value;
        StringBuilder sb = new StringBuilder();
        sb.Append("TestOptions\n");
        sb.Append($"opt_key1 = {testOption.opt_key1}\n");
        sb.Append($"Testoptions.opt_key2.k1 = {testOption.opt_key2.k1}\n");
        sb.Append($"Testoptions.opt_key2.k2 = {testOption.opt_key2.k2}");

        await context.Response.WriteAsync(sb.ToString());
    });
});

app.Run();
