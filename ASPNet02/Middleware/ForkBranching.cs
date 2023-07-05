namespace ASPNet02.Middleware
{
    public static class ForkBranching
    {
        public static void ForkBranchAdmin(this IApplicationBuilder app)
        {
            app.Map("/admin", (app1) =>
            {
                // Cần tạo Middleware của nhánh này, tạo middleware giống hệt cách làm với nhánh chính
                app1.UseRouting();

                app1.UseEndpoints(endpoint =>
                {

                    // Branch endpoint 1
                    endpoint.MapGet("/user", async context =>
                    {
                        await context.Response.WriteAsync("Trang quan ly user - CRUD");
                    });

                    // Branch endpoint 2
                    endpoint.MapGet("/user/index.html", async context =>
                    {
                        await context.Response.WriteAsync("Danh sach tai khoan user");
                    });

                    // Branch endpoint 3
                    endpoint.MapGet("/product", async context =>
                    {
                        await context.Response.WriteAsync("Trang quan ly san pham");
                    });
                });

                app1.Run(async context =>
                {
                    await context.Response.WriteAsync("Trang admin");
                });
            });
        }

        public static void ForkBranchManager(this IApplicationBuilder app)
        {
            app.Map("/manager", (app1) =>
            {
                // Cần tạo Middleware của nhánh này, tạo middleware giống hệt cách làm với nhánh chính

                app1.UseRouting();

                app1.UseEndpoints(endpoint =>
                {
                    // Branch endpoint 1
                    endpoint.MapGet("/product", async context =>
                    {
                        await context.Response.WriteAsync("Trang quan ly san pham cua Manager");
                    });
                });

                // Nếu đã rẽ nhánh vào đây mà url không khớp với endpoints trên thì sẽ vào endpoints cuối này
                app1.Run(async context =>
                {
                    await context.Response.WriteAsync("Trang Manager");
                });
            });
        }
    }
}