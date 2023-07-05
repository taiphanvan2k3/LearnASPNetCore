namespace ASPNet02.Middleware
{
    public class FirstMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// RequestDelegate tương đương với async (HttpContext context) => {}
        /// </summary>
        /// <param name="next"></param>
        public FirstMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Phương thức này sẽ được gọi khi HttpContext đi qua Middleware này trong pipeline
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            System.Console.WriteLine($"URL: {context.Request.Path}");

            context.Items.Add("DataFirstMiddleware", $"<p>URL: {context.Request.Path}</p>");
            
            // Chèn thêm nội dung vào Response, tuy nhiên nếu đã viết nội dung vào Middleware rồi mà
            // tại SecondMiddleware lại add header thì sẽ phát sinh ngoại lệ. Do đó ta phải truyền
            // dữ liệu thông qua Items
            // await context.Response.WriteAsync($"<p>URL: {context.Request.Path}</p>");

            // Chuyển HttpContext này cho các middleware phía sau xử lí tiếp
            // Nếu không chuyển HttpContext này cho Middleware ở sau thì nó được gọi
            // là terminal Middleware
            await _next(context);
        }
    }
}