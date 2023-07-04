var builder = WebApplication.CreateBuilder(args);

// Thiết lập chạy trên localhost có port nào tùy ý
// Nếu không thiết lập thì nó sẽ sử dụng port được thiết lập trong
// launchSetting.json
builder.WebHost.UseUrls("http://localhost:8080");


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
app.UseStaticFiles(); 

app.UseRouting();

// Không chỉ định kiểu cũng được
app.UseEndpoints((IEndpointRouteBuilder endpoints) =>
{
    // GET /
    endpoints.MapGet("/", async (context) =>
    {
        // Trong đối tượng context chứa Request thuộc lớp HttpRequest
        // và Response thuộc lớp HttpResponse
        // context là 1 đối tượng thuộc RequestDelegate và sẽ trả về 1 task
        // do đó mới dùng async 

        string html = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""UTF-8"">
                    <title>Trang web đầu tiên</title>
                    <link rel=""stylesheet"" href=""/css/bootstrap.min.css"" />
                    <script src=""/js/jquery.min.js""></script>
                    <script src=""/js/popper.min.js""></script>
                    <script src=""/js/bootstrap.min.js""></script>


                </head>
                <body>
                    <nav class=""navbar navbar-expand-lg navbar-dark bg-danger"">
                        <div class = ""container-fluid"">
                            <a class=""navbar-brand"" href=""#"">Brand-Logo</a>
                            <button class=""navbar-toggler"" type=""button"" data-toggle=""collapse"" data-target=""#my-nav-bar"" aria-controls=""my-nav-bar"" aria-expanded=""false"" aria-label=""Toggle navigation"">
                                    <span class=""navbar-toggler-icon""></span>
                            </button>
                            <div class=""navbar-collapse collapse d-sm-inline-flex justify-content-between"" id=""my-nav-bar"">
                                <ul class=""navbar-nav flex-grow-1"">
                                    <li class=""nav-item active"">
                                        <a class=""nav-link"" href=""#"">Trang chủ</a>
                                    </li>
                                
                                    <li class=""nav-item"">
                                        <a class=""nav-link"" href=""#"">Học HTML</a>
                                    </li>
                                
                                    <li class=""nav-item"">
                                        <a class=""nav-link disabled"" href=""#"">Gửi bài</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </nav> 
                    <p class=""display-4 text-danger"">Đây là trang đã có Bootstrap</p>
                </body>
                </html>";
        await context.Response.WriteAsync(html);
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
