using ASPNet06.MailUtils;
using ASPNet06.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddOptions();

// Đăng kí dịch vụ cho option service
services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// Đăng kí dịch vụ cho service không phải option service
services.AddTransient<SendMailService>();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello world");
    });

    // Endpoints này sẽ không gửi mail được vì máy đang chạy không có MailTransporter
    endpoints.MapGet("/TestSendMail", async context =>
    {
        var message = await MailUtils.SendMail("taiphanvan2403@gmail.com", "xuanthulab.net@gmail.com",
                    "Test", "Xin chào XuanThuLab");
        await context.Response.WriteAsync(message);
    });

    // Có thể sử dụng 1 trong 2 cách gửi mail sau
    // c1:
    endpoints.MapGet("/TestGmail", async context =>
    {
        var message = await MailUtils.SendGmail("taiphanvan2403@gmail.com", "taiphanvan2403@gmail.com",
                    "Test thử tính năng gửi email của Google", "Xin chào Phan Văn Tài. Đã gửi email thành công rồi nè :)", "porwcadxreicnrjb");
        await context.Response.WriteAsync(message);
    });

    // c2: sử dụng Mailkit để gửi mail, thường được dùng nhiều hơn.
    endpoints.MapGet("/TestSendMailService", async context =>
    {
        var sendMailService = context.RequestServices.GetService<SendMailService>();
        var mailContent = new MailContent()
        {
            To = "taiphan2403a@gmail.com",
            Subject = "Test gửi email Mailkit",
            Body = $@"<h1>Kiểm tra thử gửi email {DateTime.Now}</h1>
                <p><i>xin chào Phan Văn Tài</i></p>
                <img src=""https://lh3.googleusercontent.com/a/AAcHTtdwVS2bfhp8RUD2HrhdiNYManvSE2lgpHz_A89Jq3FKoxA=s40-p""/>"
            };

        var message = await sendMailService.SendMail(mailContent);
        await context.Response.WriteAsync(message);
    });
});

app.Run();
