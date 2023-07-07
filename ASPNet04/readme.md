**I. Lí thuyết mới và ôn tập kiến thức**
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

**II. Quan trọng**:  
1. Launch ứng dụng với https
```json
"profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5146",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:7122;http://localhost:5146",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
}
```
- Mặc định khi chạy dotnet watch run thì `mặc định` sẽ chạy ứng dụng web với `profile http`.
Lúc này do `"applicationUrl": "http://localhost:5146"` nên ta chỉ có thể truy cập ứng dụng bằng giao thức http. Nếu chỉnh lại `"applicationUrl"` thành https thì ứng dụng vẫn sẽ chạy được ở https nhưng không chuyển qua http được và về mặt ý nghĩa thì không đúng lắm Nếu thay đổi giao thức khi đang chạy với profile này là https thì sẽ không được.  
=> Để khắc phục là khi ứng dụng đã chạy thì vừa có thể chạy bằng http, vừa chạy bằng https ta sử dụng: `dotnet watch run --launch-profile https`. Có thể thay https thành tên của 1 profile này đó trong `session "profiles"`
2. Nếu khi chạy với Https mà xuất hiện lỗi `StatusCode cannot be set because the response has already started` trong khi chạy với dotnet watch run (giao thức http) thì không bị. Vấn đề là do việc WriteAsync() trong các Middleware của pipeline. Một khi đã WriteAsync thì response sẽ bắt đầu được gửi về client nhưng khi chuyển HttpContext đó đến Middleware tiếp theo thì Middleware này lại tiếp tục WriteAsync là thiết lập HttpStatusCode là 200 nên sẽ bị lỗi. Tuy nhiên `lỗi này có lúc xảy ra có lúc không xảy ra`. Nên `tốt nhất là các Middleware chưa phải là terminate Middleware thì nên truyền dữ liệu vào dictionary Items để terminate Middleware lấy ra rồi WriteAsync 1 lần`. `(Các Middleware có thể WriteAsync được nhiều lần)`  
- Bằng cách chỉ sử dụng `WriteAsync trong terminal middleware`, bạn đảm bảo rằng `phản hồi chỉ được gửi một lần` và `không có xung đột với các middleware khác` trong pipeline. `Middleware trước đó` trong chuỗi pipeline sẽ tiếp tục `chuyển yêu cầu và phản hồi đến middleware tiếp theo` bằng cách gọi phương thức Next `mà không gửi phản hồi trực tiếp`.
