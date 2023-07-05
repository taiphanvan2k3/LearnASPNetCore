using ASPNet02.Middleware;

var builder = WebApplication.CreateBuilder(args);

var service = builder.Services;
service.AddSingleton<SecondMiddleware>();

var app = builder.Build();

// Nếu url đúng là đường dẫn đến file trong wwwroot thì nó response luôn
// mà không chuyển đến các Middleware tiếp theo
app.UseStaticFiles();

// Extension method
app.UseFirstMiddleware();
app.UseSecondMiddleware();

// EndpointRoutingMiddleware
app.UseRouting();

// Sẽ response HttpContext mà không chuyển đến M1, nếu địa chỉ url thỏa mãn với 1 trong các endpoints chỉ ra
app.UseEndpoints(endpoint =>
{
    // Endpoint 1
    endpoint.MapGet("/about.html", async context =>
    {
        await context.Response.WriteAsync("Trang gioi thieu");
    });

    // Endpoint 2
    endpoint.MapGet("/sanpham.html", async context =>
    {
        await context.Response.WriteAsync("Trang san pham");
    });
});

// Sau khi HtppContext chưa bị response khi đi qua các Middleware trên
// thì ta tạo ra 1 rẽ nhánh pipeline
app.ForkBranchAdmin();

// Nhánh manager
app.ForkBranchManager();

// Đây là terminal Middleware
app.Run(async context =>
{
    await context.Response.WriteAsync("Xin chao ASP.NET core");
});

// Pipeline: StaticMiddleware (->|stop) FirstMiddleware -> SecondMiddleware (->|stop) EndpointRoutingMiddleware (->|stop) M1
app.Run();
