# Các package cần sử dụng khi làm việc với EF sử dụng Sqlserver:
- dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
- dotnet add package Microsoft.EntityFrameworkCore.Design
- dotnet add package Microsoft.EntityFrameworkCore.SqlServer
- Có thể cài thêm: `dotnet tool install --global dotnet-aspnet-codegenerator` nếu chưa có. Hiện tại nếu có rồi thì sẽ thông báo: `Tool 'dotnet-aspnet-codegenerator' is already installed.`
- localhost <=> 1270.0.0.1
# Attribute:
- Vd có property là DateTime và muốn thiết lập dữ liệu nó lưu dưới DB là Date thì có thể dùng 1 trong 2 cách:
    + `[Column(Typename="date")]`
    + `[DataType(DataType.Date)]`

# Entity framework:
- `dotnet-ef database drop -f` để xóa Db mà không hỏi yes/no
- Có thể tạo ra dữ liệu mẫu bằng thư viện Bogus: `dotnet add package Bogus --version 34.0.2`
- `dotnet aspnet-codegenerator razorpage -m RazorWebTongHop.Models.Article -dc RazorWebTongHop.Models.DataContext -udl -outDir Pages/Blog --referenceScriptLibraries` để tạo ra các thao tác CRUD cho model Article sử dụng DataContext. 
    + Lưu ý: Để chạy được thì cài thêm package: `Microsoft.EntityFrameworkCore.Tools`
- Nếu tạo 1 page trong 1 thư mục nào đó: dotnet new page -n `<tên page>`  -o `<thư mục lưu>` -p:n (hoặc --namespace) `<tên namespace>`  
Ví dụ: `dotnet new page -n Create -o Pages/BlogMySelf --namespace RazorWebTongHop.Pages.BlogMySelf`

# Ghi chú:
- Để không cho người dùng edit 1 input nào đó nhưng vẫn muốn giá trị của nó được submit thì dùng `readonly="readonly"` cho thẻ input đó
-  Khi dùng `<td>@Html.DisplayFor(model => article.CreateAt)</td>` sẽ lấy giá trị của property CreateAt đồng thời kết hợp thêm các DataAnnotation để hiển thị, cụ thể giá trị dưới CSDL có cả time nhưng ở property là Date nên nó sẽ chỉ hiển thị Date