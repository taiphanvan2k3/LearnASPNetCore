# Razor page:
- Các trang razor page (.cshtml) viết code html + C#
+ Các file này sẽ được các engine razoz biên dịch và trả về cho client.
Mặc định các razor page được lưu trong thư mục Pages. Các trang file cshtml để đánh dấu là razor page cần có @page
- Khi sử dụng `app.MapRazorPages();` thì tìm trên toàn bộ mã nguồn các trang razor page cshtml và nó sử dụng các rang này như các endpoints
Vd có 1 file cshtml FirstPage.cshtml và có chỉ thị @page thì nó sẽ tạo ra 1 endpoints có url là: /FirstPage 
- Khai báo biến bên trong @{ } thì bên ngoài { } vẫn dùng lại được
- Thay đổi url đến 1 trang razor page:
+ C1:
```csharp
services.AddRazorPages().AddRazorPagesOptions(options =>
{
    // Đổi thư mục mặc định lưu các trang razor page
    options.RootDirectory = "/Pages";

    // Viết lại các url truy cập đến razor page, mặc định là /<tên file razor>
    options.Conventions.AddPageRoute("/FirstPage", "/trang-dau-tien.html");
    options.Conventions.AddPageRoute("/SecondPage", "/MyPages/trang-thu-hai.html");
    ....
})
```
=> Thì lúc này muốn truy cập đến razor page: FirstPage.cshtml ta có thể truy cập thông qua `/FirstPage` hoặc `/trang-dau-tien.html`
+ C2: sau @Page thì thêm "url" vào vd ở ThirdPage.cshtml ta thêm vào sau `@Page` "/trang-thu-ba/hihi". Lúc này chỉ có thể truy cập đến trang razor đó thông qua 
url: /trang-thu-ba/hihi
- Thêm tham số vào url thiết lập sau @Page: vd `/trang-dau-tien/{loop-number:int}`
+ Để truy cập được đến FirstPage.cshtml thì cần truy cập url: /trang-dau-tien/2 ,... Nếu tham số là optional thì `/trang-dau-tien/{loop-number:int?}`
- Có thể tạo thêm thư mục con trong Pages. Lúc này EndpointRoutingMiddleware sẽ tự động tạo url đến các trang razor page bên trong thư mục con đó. Tương tự thì ta vẫn có thể thiết lập url đến các trang razor đó như bình thường
+ Nếu trang razor page là Index.cshtml thì khi truy cập /DichVu thì nó sẽ tự động tìm file Index.cshtml nếu có
# Tag helper
- Để phát sinh html ta cần nạp vào các trang razor page nào cần dòng: `@addTagHelper *,Microsoft.AspNetCore.Mvc.TagHelpers`
+ Tình huống:
```html
<!-- Tại trang Index.cshtml của DichVu, muốn đi đến trang DichVu1.cshtml cần chỉ
rõ url đến đúng trang này. Tuy nhiên nếu sau này đổi url tại trang DichVu1.cshtml thành một url khác thì href này không đúng nữa -->
<a href="/DichVu/service1">Đi đến dịch vụ 1</a>
```
Thay vào đó ta dùng tag-helper `asp-page` vd asp-page="DichVu1" thì nó sẽ tự động sinh ra href = "/DichVu/service1". Chú ý rằng Index.cshtml phải cùng cấp
với file DichVu1.cshtml mới phát sinh href được. Còn không href nó rỗng. Dù thay đổi url của razor page thì asp-page tự động cập nhật lại href
# Area
- Có thể gôm nhóm các trang razor thành các từng thư mục trong Area. Lúc này EndpointRoutingMiddleware cũng tự động sinh ra url đến file razor trong area theo cấu trúc: `<tên area>/<tên file cshtml>`. Tương tự như trong thư mục Pages thì nếu trong /Areas/Product có
file Index.cshtml thì khi truy cập url là /Product thì nó tự động vào Index.cshtml. Việc thiết lập url sau @page tương tự như ở Pages
- Chú ý để dùng tag helper để href đến 1 trang razor trong 1 area ta cần lưu ý sau:
```html
<!-- Phải có / đằng trước NewProduct -->
<a asp-page="/NewProduct" asp-area="Product">Đi đến trang sản phẩm mới</a>
```
+ Tuy nhiên nếu href đến 1 file nằm trong cùng thư mục với nó thì có thể làm:
```html
<a asp-page="NewProduct">Đến trang sản phẩm mới</a>
<br>
<a asp-page="/NewProduct">Đến trang sản phẩm mới 2</a>
<br>
<a asp-page="/NewProduct" asp-area="Product">Đến trang sản phẩm mới 3</a>
```
# Bản chất của biên dịch file cshtml:
- Về bản chất khi ta biên dịch thì thư viện .Net Razor Page sẽ đọc toàn bộ file cshtml. Từ nội dung file đó sẽ xây dựng một lớp có các thuộc tính và phương thức mà khi chạy phương thức đó thì sẽ xuất ra code html mà ta viết
- Có thể tạo phương thức, thuộc tính trong file cshtml:
```cs
@* Các phương thức *@
@functions {
    string title { get; set; } = "Đây là trang razor page đầu tiên";
    int number = 10;
    @* Có thể viết luôn phương thức vào đây hoặc tách riêng ra 1 phương thức khác *@
}

@functions {
    string Welcome(string name)
    {
        return $"Chào mừng {name} đến với website";
    }

    void PrintItem(int soLanLap)
    {
        <p> Đây là phương thức tự xây dựng trong file cshtml. Có thể dùng được ở mọi nơi trong file này.</p>
        <p>=> Kết quả lặp là: </p>
        <ul>
            @for (int i = 1; i <= soLanLap; i++)
            {
                <li>Item thứ @i</li>
            }
        </ul>
    }
    // In ra
    <p>Dùng phương thức để in ra:</p>
    @{
        this.PrintItem(3);
    }
}
```

- ViewData: đối tượng này chứa dữ liệu để truyền giữa các phương thức.
+ ViewData["key"] = data;

# Tách riêng code xử lí, không viết ỏ file cshtml
## Code trước khi tách
```csharp
@page "/trang-dau-tien/{loop-number:int?}"
@{
    @* ViewData["mydata"] = "Tự học ASP.Net 08/07/2023"; *@
}

<h1 style="color: red;">@this.title</h1>
<h2 style="font-size: 20px; color: green;">@ViewData["mydata"]</h2>
<p>Học lập trình ASPNet Core (<strong>@DateTime.Now</strong>)</p>
<p>@this.Welcome("Phan Văn Tài")</p>

@{
    var a = 2;
    int b = 3;
    <p>Căn bậc 2 của @a là @Math.Sqrt(a)</p>
    <p>Giá trị của biến number đã khai báo trong function là @this.number</p>
    var loop_number = this.Request.RouteValues["loop-number"] ?? "0";
    int soLanLap = Int32.Parse(loop_number.ToString());
}

@* Có thể dùng biến ở ngoài @{ } *@
<p>Tổng của a và b là @(a + b)</p>
<p>Số lần lặp là @soLanLap</p>
<ul>
    @for (int i = 1; i <= soLanLap; i++)
    {
        <li>Item thứ @i</li>
    }
</ul>

<p>Dùng phương thức để in ra:</p>
@{
    this.PrintItem(3);
}

@* Các phương thức *@
@functions {
    string title { get; set; } = "Đây là trang razor page đầu tiên";
    int number = 10;
    @* Có thể viết luôn phương thức vào đây hoặc tách riêng ra 1 phương thức khác *@
}

@functions {
    string Welcome(string name)
    {
        return $"Chào mừng {name} đến với website";
    }

    void PrintItem(int soLanLap)
    {
        <p> Đây là phương thức tự xây dựng trong file cshtml. Có thể dùng được ở mọi nơi trong file này.</p>
        <p>=> Kết quả lặp là: </p>
        <ul>
            @for (int i = 1; i <= soLanLap; i++)
            {
                <li>Item thứ @i</li>
            }
        </ul>
    }

    // Các handler
    public void OnGet()
    {
        // Khi truy vấn đến trang thì nó tự động gọi phương thức này đầu tiên
        // sau đó nó mới chạy từ đầu file đến
        System.Console.WriteLine("Truy van bang phuong thuc Get");
        ViewData["mydata"] = $"Tự học Razor page 08/07/2023";
        // Thread.Sleep(1000);
        // ViewData này sẽ bị ghi đè nếu ta khởi tạo lại ở trên, Thread.Sleep sẽ thấy
    }

    // Khi truy cập url có dạng: url?handler=xyz
    public void OnGetXyz()
    {
        System.Console.WriteLine("Truy van bang phuong thuc onGetXyz");
        ViewData["mydata"] = $"Tự học Razor page 08/07/2023 XYZ";
    }

    // Tương tự ta có OnPost(), OnPostAbc()
}
```
# Layout
- Trước khi có _Layout thì khi truy cập đến 1 trang razor page thì nội dung file là gì thì nó response lại cho trình duyệt như vậy. Và bây giờ mong muốn rằng code phát sinh không trả trực tiếp về cho client mà trả về 1 trang cshtml khác. Trang cshtml khác đó sẽ có nhiệm vụ phát sinh code và trả về cho client
- Đặt tên layout có _ ở đầu để EndpointRoutingMiddleware không sử dụng file đó để tạo ra điểm endpoints
- File này ta sẽ viết ra 1 cấu trúc Html hoàn chỉnh, cú pháp tạo nhanh: sử dụng emmet `html`
- Dùng file layout này có nhiều điểm lợi:
+ Không còn xuất hiện lỗi `unenable aspnetcore-browser-refresh.js`
+ Chèn nội dung css vào file layout này
- `Có thể tạo nhiều layout`, file cshtml nào dùng layout nào thì sử dụng layout đó thông qua:
```csharp
@{
    Layout = "_MyLayout";
}
```
- Dùng _ViewStart, _ViewImports: nội dung này sẽ được tự động chèn vào đầu mỗi khi truy cập đến 1 file
+ ViewStart: ở trong file này thường dùng để thiết lập Layout chung cho các razor page. Tuy nhiên nếu file cshtml nào muốn thiết lập lại layout thì nó có thiết lập lại thông qua 
```csharp
@{
    Layout = "_MyLayout";
}
```
đặt tại đầu file của nó (cho dễ hiểu là file này sử dụng Layout nào ấy mà). Sau khi .Net đọc xong nội dung file này nó sẽ quyết định chọn Layout nào hay không. Nếu có thiết lập Layout (hoặc sử dụng Layout đã dùng ở _ViewStart.cshtml hoặc ghi đè lại Layout) thì nó sẽ đổ dữ liệu này vào file cshtml Layout tương ứng rồi mới response về cho trình duyệt.
+ _ViewImports: dùng để thêm các chỉ thị addTagHelper, using (using Models, Dtos,...), inject