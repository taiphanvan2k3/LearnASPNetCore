- Để tạo ra những option, configure riêng thì ta hay tạo ra các session trong appsettings.json
    + Các session thiết lập các giá trị dưới dạng key, value, trong đó value có thể là 1 session khác
    + Đối tượng Configuration được khởi tạo và đăng kí vào hệ thống kiểu Transient. Lấy nó ra thông qua `builder.Configuration` hoặc thông qua đối tượng `HttpContext` như sau:  
    ```csharp
    var configuration = context.RequestServices.GetService<IConfiguration>();
    ```
- Một service khi đã add vào service thì có thể được inject vào 1 service khác hoặc dùng để inject vào Middleware, controller, API, ...
- Kĩ thuật inject DI có 2 loại:
    + Inject 1 option (option này sẽ là 1 service dùng để `ánh xạ các key,vlaue của các session` bên trong file `appsetting.json` vào các property của nó): thông qua việc add nó vào services thông qua phương thức `Configure<{Tên option}>`  
    Ví dụ:  
    ```csharp
    services.Configure<TestOption>(testOption);
    ``` 
    Lúc này nó sẽ add vào services 1 service có kiểu là `IOption<TestOption>`. Lúc này dùng thì inject service `IOption<TestOption>` vào. Để lấy ra TestOption thì truy cập vào property `Value` của đối tượng `IOption<TestOption>`
    + Inject 1 service: thông qua việc:
    ```csharp
    services.AddSingleton<ProductNameService>();
    ```
- Nhắc lại: các Middleware tạo ra bằng cách kế thừa từ IMiddleware thì phải add nó vào hệ thống dưới dạng services thì sau đó
mới có thể sử dụng
```csharp
// Add Middlware vào dưới dạng service vào hệ thống
// Do đây là ta tạo 1 Middleware bằng cách implement từ  IMiddleware nên mới phải add vào service. 
// Còn cách Middleware tạo theo cách không implement IMiddleware thì không cần add service 
// mà chỉ cần app.UseMiddleware<T>() là được.
services.AddTransient<TestOptionMiddleware>();

// Nhắc lại: tất cả việc add service phải thực hiện trước khi builder.Build()
var app = builder.Build();

// Request phải đi qua Middleware TestOptionMiddleware trước khi xuống 
// EndpointRoutingMiddleware
app.UseMiddleware<TestOptionMiddleware>();
```