using ASPNet02.Middleware;

namespace ASPNet02.Middleware
{
    public static class UseFirstMiddlewareMethod
    {
        /// <summary>
        /// Đưa vào pipeline FirstMiddleware
        /// </summary>
        /// <param name="app"></param>
        public static void UseFirstMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<FirstMiddleware>();
        }

        public static void UseSecondMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SecondMiddleware>();
        }
    }
}