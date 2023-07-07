using ASPNet05.ProductManage;
using Microsoft.Extensions.Caching.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add dịch vụ:
var services = builder.Services;
// Kích hoạt session

// Lưu trữ session trong cache
// services.AddDistributedMemoryCache();

services.AddDistributedSqlServerCache((SqlServerCacheOptions option) =>
{
    option.ConnectionString = "Data Source=localhost,1433; Initial Catalog=appMVC; User ID=sa;Password=taiphan2403;TrustServerCertificate=true";
    option.SchemaName = "dbo";
    option.TableName = "MySession";
});

services.AddSession((options) =>
{
    options.Cookie.Name = "MyCookie07072023";
    // Thiết lập thời gian có hiệu lực của session. Mỗi lần GetInt, GetString hay Set,... thì cột Expire at time của session sẽ
    // được update
    // Sau thời gian này nếu cookie vẫn được gửi lên thì dữ liệu của Session không được phục hồi
    // và nó bị xóa đi và khởi tạo ra 1 session khác
    options.IdleTimeout = new TimeSpan(0, 2, 0);
});

var app = builder.Build();

// SessionMiddleware
app.UseSession();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        int? count = context.Session.GetInt32("count");
        if (count == null)
        {
            count = 0;
        }

        await context.Response.WriteAsync($"So lan truy cap vao trang /read-write-sessions tại Home: {count}");
    });

    endpoints.MapGet("/read-write-sessions", async context =>
    {
        // Đếm số lần truy cập vào session
        int? count = context.Session.GetInt32("count");
        if (count == null)
        {
            count = 0;
        }

        count++;
        // Lưu session
        context.Session.SetInt32("count", count.Value);

        // Thường dùng SetString để lưu trữ dữ liệu của các đối tượng khi chuyển về json
        // context.Session.SetString("","");
        await context.Response.WriteAsync($"So lan truy cap vao trang /read-write-sessions: {count}");
    });

    endpoints.MapGet("/add-product", async context =>
    {
        string result = ProductController.BuyMoreProduct(context);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(result);
    });

    endpoints.MapGet("/view-products", async context =>
    {
        string result = ProductController.ViewProducts(context);
        await context.Response.WriteAsync(result);
    });
});

app.Run();

/* Tạo ra bảng trong DB để lưu session
dotnet sql-cache create "Data Source=localhost,1433; Initial Catalog=appMVC; User ID=sa;Password=taiphan2403;TrustServerCertificate=true" dbo MySession
*/