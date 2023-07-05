using ASPNet03.utils;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoint =>
{
    endpoint.MapGet("/", async context =>
    {
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        string html = HtmlHelper.HtmlDocument("Xin chao", menu + HtmlHelper.HtmlTrangchu());
        await context.Response.WriteAsync(html);
    });

    endpoint.MapGet("/RequestInfo", async context =>
    {
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        var info = RequestProcess.RequestInfo(context.Request).HtmlTag("div", "container");
        string html = HtmlHelper.HtmlDocument("Thông tin request", menu + info);
        await context.Response.WriteAsync(html);
    });

    endpoint.MapGet("/Encoding", async context =>
    {
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);
        await context.Response.WriteAsync("Encoding");
    });

    // Khi thiết lập url như sau thì truy cập có/không có chuỗi truy cập phía sau đều được
    // Vd truy cập được: /Cookies/abc hoặc /Cookies/abc/cde/xyz,...
    endpoint.MapGet("/Cookies/{*action}", async context =>
    {
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);

        // Nếu không có action thì nó sẽ trả về null do đó ta mới dùng ?? để tạo giá trị cho nó khi null
        var action = context.GetRouteValue("action") ?? "read";

        string message = "";
        if (action.ToString() == "write")
        {
            // Tên - giá trị - thời gian hiệu lực
            var option = new CookieOptions()
            {
                Path = "/",
                Expires = DateTime.Now.AddDays(1)
            };

            context.Response.Cookies.Append("masanpham-new-2", Guid.NewGuid().ToString(), option);
            message = "Cookie được ghi";
        }
        else
        {
            var listCookiesString = context.Request.Cookies.Select((header) =>
            {
                return $"{header.Key}: {header.Value}".HtmlTag("li");
            });
            message = string.Join("", listCookiesString);
        }

        var huongDan = @"<a class=""btn btn-danger"" href= ""/Cookies/read"">Đọc cookie</a>
           <a class=""btn btn-success"" href= ""/Cookies/write"">Ghi cookie</a>".HtmlTag("div", "container mt-4"); // mt-4 : margin-top: 4 phần
        message = message.HtmlTag("div", "alert alert-danger mt-2");
        var html = HtmlHelper.HtmlDocument("Cookie: " + action, menu + huongDan + message);
        await context.Response.WriteAsync(html);
    });

    endpoint.MapGet("/Json", async context =>
    {
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);

        var sanpham = new
        {
            TenSanPham = "Laptop thinkpad",
            Gia = 31000,
            NgaySX = new DateTime(2022, 12, 21)
        };

        // Thiết lập như vậy thì trình duyệt sẽ hiển thị nội dung trả
        // về dưới dạng json nên HTML không hiển thị được

        // Thiết lập header trả về
        context.Response.ContentType = "application/json";

        var json = JsonConvert.SerializeObject(sanpham).HtmlTag("p");
        string html = HtmlHelper.HtmlDocument("JSON", menu + json);
        await context.Response.WriteAsync(html);
    });

    endpoint.MapMethods("/Form", new string[] { "POST", "GET" }, async context =>
    {
        var menu = HtmlHelper.MenuTop(HtmlHelper.DefaultMenuTopItems(), context.Request);

        var formHtml = await RequestProcess.ProcessSubmitFormAsync(context.Request);

        string html = HtmlHelper.HtmlDocument("Test submit form html", menu + formHtml);
        await context.Response.WriteAsync(html);
    });
});

app.Run();
