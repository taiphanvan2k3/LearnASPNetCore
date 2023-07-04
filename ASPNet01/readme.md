# Một số lưu ý khi chuyển code từ Net 5.0 sang Net 6.0
- Code của file Startup.cs và Program.cs trong Net5.0 đã gộp thành top-level đặt ở Program.cs Net6.0
- Code viết trong phương thức ```public static IHostBuilder CreateHostBuilder(string[] args)``` tại Program.cs ở Net5.0 thì bây giờ viết ngay sau khi khởi tạo builder
- Code của ```Startup.ConfigureServices``` thì đăng kí dịch vụ thông qua đối tượng builder.Services, xử lí trước khi gọi builder.Build()
- Code trong ```Startup.Configure```: Tại đây chủ yếu để thêm các middleware vào pileline, cấu hình routing ... Toàn bộ code này giờ đây sẽ đặt sau đoạn code ```builder.Build()```:

``` 
var app = builder.Build();
/* ============================================================
    Code viết trong Configure cũ đặt tại đay, ví dụ:
   =========================================================== */
```
# Kiến thức:
- Trong ứng dụng ASP.Net khi nhận được các HTTP message request thì các request này phải được chuyển xử lí và đi qua các đoạn code để trả về các http message response. Những thành phần mà request phải đi qua gọi là pipeline (là 1 chuỗi Middleware)
- Đối tượng app trong file Program.cs cho phép ta tạo ra các Middleware cuối cùng
+ Thêm 1 terminate Middleware
```
app.Run(async context =>
{
    await context.Response.WriteAsync("Xin chao day la Startup");
});
```
+ Mọi request đều phải đi qua terminate middleware trước khi nó được respone. Terminate middleware là Middleware sẽ không gọi một Middleware khác sau khi request đi qua nó
+ Ví dụ sau đây tạo ra 2 terminate middleware
```
<!-- Nếu địa chỉ truy cập là /abc thì terminate middleware này sẽ được truy cập
và trả về kết quả ngay. Còn nếu không phải thì nó sẽ gọi terminate middleware ở dưới -->
app.Map("/abc", app1 =>
{
    app1.Run(async context =>
    {
        await context.Response.WriteAsync("Noi dung tra ve tu ABC");
    });
});

<!-- Middleware này luôn thỏa mãn với các địa chỉ truy cập -->
app.Run(async context =>
{
    await context.Response.WriteAsync("Xin chao day la Startup");
});
```

- Thực tế ít dùng các tạo các endpoints viết trực tiếp bằng 2 cách như trên mà sẽ sử dụng Middleware routing thông qua phương thức ```app.UseRoutings()```. Phương thức này đưa vào pipeline 1 middleware tên là ```EndpointRoutingMiddleware```. Khi request đi qua middleware này thì nó sẽ phân tích địa chỉ truy cập và điều hưóng request đó đi theo 1 luồng đi đến 1 endpoints nhất định. 
+ Sau khi sử dụng ```app.UseRoutings()``` ta cần sử dụng 
```
app.UseEndpoints(endpoints => {

});
``` 
để xây dựng ra các điểm endpoints. Bất khi 1 request gửi đến nó sẽ đi qua middleware ```EndpointRoutingMiddleware``` này và nó sẽ phân tích địa chỉ truy cập để điều hướng đến các endpoints đã xây dựng trong ```app.UseEndpoints```. Nếu địa chỉ request truy cập không phù hợp với các điểm endpoints đã chỉ ra trong ```app.UseEndpoints``` thì request sẽ đi qua routing, không sử dụng 1 endpoints nào cả và đi qua tiếp đến các điểm rẽ nhánh của pipeline (cụ thể là nó sẽ đi xuống các terminate middleware bên dưới)
+ Trong thực tế sẽ không dùng terminate middleware sau:
```
app.Run(async context =>
{
    await context.Response.WriteAsync("Xin chao day la Startup");
});
```
Vì nó luôn thỏa mãn với bất kì địa chỉ truy cập nào. Điều ta mong muốn là nếu địa chỉ truy cập không phù hợp thì sẽ hiển thị NotFound. Mà ta sẽ thêm 1 ```StatusCodePages middleware``` vào pipeline thông qua ```app.UseStatusCodePages();``` Đây là middleware cuối cùn trong pipleline khi các middleware khác không xử lí các request gửi đến. Ví dụ khi truy cập đến /abcde thì middleware StatusCodePages sẽ trả về ```Status Code: 404; Not Found```

- ```app.UseStaticFiles();``` cho phép truy cập vào các file tĩnh được đặt trong thư mục wwwroot