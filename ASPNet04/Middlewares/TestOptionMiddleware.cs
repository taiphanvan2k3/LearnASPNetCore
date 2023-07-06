using System.Text;
using ASPNet04.Options;
using ASPNet04.Services;
using Microsoft.Extensions.Options;

namespace ASPNet04.Middlewares
{
    public class TestOptionMiddleware : IMiddleware
    {
        // Inject TestOption vào để dùng
        private readonly TestOption _testOption;
        private readonly ProductNameService _productNameService;

        // Inject 1 option nên phải inject IOptions<T>
        public TestOptionMiddleware(IOptions<TestOption> testOption, ProductNameService productNameService)
        {
            _testOption = testOption.Value;
            _productNameService = productNameService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("Show options in TestOptionsMiddleware\n");

            StringBuilder sb = new StringBuilder();
            sb.Append("TestOptions\n");
            sb.Append($"opt_key1 = {_testOption.opt_key1}\n");
            sb.Append($"Testoptions.opt_key2.k1 = {_testOption.opt_key2.k1}\n");
            sb.Append($"Testoptions.opt_key2.k2 = {_testOption.opt_key2.k2}");

            foreach (var productName in _productNameService.GetNames())
            {
                sb.Append(productName + "\n");
            }

            await context.Response.WriteAsync(sb.ToString());
            await next(context);
        }
    }
}