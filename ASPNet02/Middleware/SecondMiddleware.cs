namespace ASPNet02.Middleware
{
    /// <summary>
    /// Kiểm tra url gửi đến có phải /xxx.html không:
    /// <br/>
    /// + Nếu có thì không gọi Middleware phía sau và trả về "Ban khong duoc truy cap" và tạo ra 1 
    /// Header SecondMiddleware: Ban khong duoc truy cap
    /// <br/>
    /// + Nếu không phải: thì tạo ra 1 header SecondMiddleware: Ban duoc truy cap va chuyen HttpContext
    /// cho Middleware phía sau
    /// </summary>
    public class SecondMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string url = context.Request.Path;
            if (url == "/xyz.html")
            {
                // Phải thiết lập header trước khi thiết lập nội dung Content cho response, nếu
                // làm ngược lại thì gây lỗi
                context.Response.Headers.Add("SecondMiddleware", "Ban khong duoc truy cap");
                var dataFromMiddleware = context.Items["DataFirstMiddleware"];
                if (dataFromMiddleware != null)
                {
                    await context.Response.WriteAsync($"<p>{dataFromMiddleware as string}</p>");
                }
                await context.Response.WriteAsync("<p>Ban khong duoc truy cap</p>");

                // Chặn không cho chuyển tiếp đến các middleware tiếp theo
            }
            else
            {
                var dataFromMiddleware = context.Items["DataFirstMiddleware"];
                if (dataFromMiddleware != null)
                {
                    await context.Response.WriteAsync($"<p>{dataFromMiddleware as string}</p>");
                }
               await context.Response.WriteAsync("<p>Ban duoc truy cap. Day la SecondMiddleware</p>");

                // Chuyển HttpContext này cho Middleware phía sau
                await next(context);
            }
        }
    }
}