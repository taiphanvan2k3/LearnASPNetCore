var builder = WebApplication.CreateBuilder(args);

// Thiết lập chạy trên localhost có port nào tùy ý
// Nếu không thiết lập thì nó sẽ sử dụng port được thiết lập trong
// launchSetting.json
builder.WebHost.UseUrls("http://localhost:5555");



// Trước khi build thì có thể đăng kí service thông qua builder.Services
/* ============================================================
    Copy code cũ trong Startup.ConfigureServices vào đây, ví dụ
    services.AddControllersWithViews();
   =========================================================== */
var services = builder.Services;


var app = builder.Build();


/* ============================================================
   Code viết trong Startup.Configure cũ đặt tại đay, ví dụ:
   =========================================================== */

// StaticMiddleware
// Nếu địa chỉ truy cập mà là đường dẫn đến 1 file trong wwwroot thì 
// nó trả về nội dung file luôn đó và request không được đi tiếp trên pipeline nữa
// Nếu file chỉ ra không có thì nó đi tiếp các middleware trong pipeline
app.UseStaticFiles();

app.UseRouting();

// Không chỉ định kiểu cũng được
app.UseEndpoints((IEndpointRouteBuilder endpoints) =>
{
    // GET /
    endpoints.MapGet("/", async (context) =>
    {
        // context là 1 đối tượng thuộc RequestDelegate và sẽ trả về 1 task
        // do đó mới dùng async 
        await context.Response.WriteAsync("Trang chu");
    });

    // endpoints.MapPost(""),...
    endpoints.MapGet("/about", async (context) =>
    {
        // context là 1 đối tượng thuộc RequestDelegate và sẽ trả về 1 task
        // do đó mới dùng async 
        await context.Response.WriteAsync("Trang gioi thieu");
    });

    endpoints.MapGet("/contact", async (context) =>
    {
        // context là 1 đối tượng thuộc RequestDelegate và sẽ trả về 1 task
        // do đó mới dùng async 
        await context.Response.WriteAsync("Trang lien he");
    });
});

// Net6.0 recommend sử dụng top-level là app.MapGet chứ không recommend sử dùn app.UseEndpoints
app.MapGet("/info", async (context) =>
{
    // context là 1 đối tượng thuộc RequestDelegate và sẽ trả về 1 task
    // do đó mới dùng async 
    await context.Response.WriteAsync("Xin chao, Toi la Phan Van Tai");
});

// Tạo ra các terminate middleware
// Các terminate này sẽ được gọi nếu địa chỉ truy cập không phù hợp với các endpoints chỉ ra ở UseEndpoints
app.Map("/abc", app1 =>
{
    app1.Run(async context =>
    {
        await context.Response.WriteAsync("Noi dung tra ve tu ABC");
    });
});

// Middleware này sẽ được gọi nếu request không đi qua terminate middleware trên
// app.Run(async context =>
// {
//     await context.Response.WriteAsync("Xin chao day la Startup");
// });

// Đưa vào pipeline 1 StatusCodePages middleware
// Đặt middleware này ở đây hoặc ở đầu cũng được vì nó chỉ được gọi nếu
// địa chỉ truy cập không thỏa mãn bất kì endpoints nào
app.UseStatusCodePages();

// Máy chủ Kestrel HTTP sẽ được chạy khi app.Run()
app.Run();
