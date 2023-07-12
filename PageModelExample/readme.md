# Ôn tập PageModel
- Mỗi file razor page có ít nhất 1 PageModel
- Nếu file razor page đơn giản thì viết @functions trong này cũng được, nhưng nếu
file razor page đã phức tạp kèm với @functions nữa thì không nên đặt @functions trong file razor mà nên tách riêng ra ở PageModel
- Khi sử dụng `@model ...` trang razor page biết được dữ liệu truyền đến nó là kiểu dữ liệu gì, chứ không đi khởi tạo đối tượng truyền đến và dữ liệu đó sẽ được gán có Model của razor page. Thường thì dữ liệu của model sẽ được khởi tạo trong các action của Controller
+ Tuy nhiên trong Razor web có đặc biệt là ta có thể liên kết @model ... với 1 class và class này có kế thừa PageModel lại. Và khi ta truy cập thì nó tự động `khởi tạo ra đối tượng của class` đó và gán vào thuộc tính Model của trang razor hiện tại. Truy cập các thuộc tính và phương thức trong PageModel thông qua đối tượng Model
- Ta có thể inject đối tượng của hệ thống vào trong PageModel
- Về đặt tên `nên` tên file cs nên đặt cùng tên với tên file Razor, chỉ thêm .cs ở cuối và tên class nên đặt có Model ở cuối
- Cú pháp cmd tạo file razor page: `dotnet new page -n ProductPage -o Pages -p:n PageModelExample.Pages`
hoặc `dotnet new page -n ProductPage -o Pages --namespace PageModelExample.Pages`
+ Trong đó n: tên của trang razor page, -o: vị trí lưu file, -na: namespace
- Các handler trong Page model có thể có tham số, nếu có tham số thì tham số này sẽ tìm trên route values của url để tự động thiết lập vào
- Chú ý:
```html
<form method="POST">
        <button class="btn btn-danger" type="submit" asp-page="ProductPage" asp-route-id="@product.Id"
            asp-page-handler="DeleteProduct">Xóa sản phẩm</button>
</form>
```
Hay dưới đây đều được
```html
<form method="POST" asp-page="ProductPage" asp-route-id="@product.Id" asp-page-handler="DeleteProduct">
    <button class="btn btn-danger" type="submit" >Xóa sản phẩm</button>
</form>
```
+ Hai cách thiết lập như trên đều được nhưng về mặt ý nghĩa thì nên thiết lập ở form vì nó giống thiết lập thuộc tính action cho form hơn
+ Không quan trong nút bấm là `<button> hay <input>` miễn có type="submit" là được. `Dùng thẻ a` thì `không được trong tình huống` này dù có cho
`thẻ <a> type = "submit"`. Đã thử click vào thẻ a thì nó refresh lại trang
+ Khi sử dụng asp-page,asp-route-id như trên thì không cần dùng thuộc tính action cho form nữa
- Để thực hiện các `phương thức Post` thì phải `submit 1 form mới được`, chứ `không đơn thuần là tạo ra url.`
- Các handler `OnGet` của PageModel, nếu kiểu dữ liệu trả về là `IActionResult`, mà `muốn hiển thị lại nội dung Page` sau khi thực hiện xong phương thức thì `chỉ cần return Page()` là đủ, vì không cần thực hiện 1 HttpRequest mới. Còn `khi thực hiện OnPost`, `cần trả về lại nội dung Page` thì `mới dùng return RedirectToPage()`

# Model binding:
- Dữ liệu gửi đến: (key, value)
- Nguồn:
    + Form HTML (Post): truy cập thông Request.Form["key"]
    + Query (Form html - get): Request.Query["key"]
    + Header: Request.Header["key"]
    + Route data vd {id:int?}: Request.RouteValues["key"]
    + Upload file
    + Body: submit json
- Đọc dữ liệu: thông qua Request, có trong controller, Page Model, View
- Trong ứng dụng ASP.Net ít khi phải đọc trực tiếp như vậy mà thông qua cơ chế binding
- Sử dụng Attribute:
    + Liên kết dữ liệu trong các tham số của action, handler: Parameter Binding
    + Liên kết dữ liệu với các property của Model: Property Binding
## Parameter Binding:
- Có handler OnGet(int? id): khi nó được thực thi, tham số khai báo sẽ được tìm trên tất cả các nguồn dược gửi đến (Form, Query, Header, RouteValues) có dữ liệu nào có key là id hay không. Nếu có thì nó sẽ tự động đọc và chuyển thành kiểu dữ liệu tương ứng. Quá trình này được diễn ra một cách tự động
+ Vd truy cập: /product/2 theo method Get thì trên RouteValues có key id -2 nên nó sẽ tự động đọc được id = 2
+ Vd truy cập: /product/2/?id=3 theo method Get thì dữ liệu id có ở trên cả 2 nguồn (RouteValues, Query). Trong trường hợp này do nó đọc được ở RouteValues trước và khi đọc được id rồi nên nó không đọc ở Query nữa nên cuối cùng id vẫn bằng 2
+ Khi truy cập: /product/?id=3 thì lúc này theo trình tự vẫn đọc trong RouteValues trước nhưng không tìm thấy và nó tiếp tục đọc ở Query và đọc được id là 3
+ Nếu id xuất hiện ở cả RouteValues và Query nhưng ta muốn chỉ rõ là nó sẽ chỉ đọc ở RouteValues hoặc chỉ ở Query thì ta chỉ rõ thêm Attribute: [FromRoute] hoặc [Fromquery]
vd : `public void OnGet([FromQuery]int? id)` thì id sẽ đọc ở query, tương tự `public void OnGet([FromRoute]int? id)` thì nó đọc được id ở RouteValue
+ Nếu tham số là id nhưng muốn khi nhập url không phải là id mà muốn là idSanPham, nhưng khi binding thì binding vào tham số có tên là id thì sử dụng thêm: `public void OnGet([FromQuery(Name = "idsanpham")]int? id)`. Khi nhập url là /product?idsanpham=2 thì sau khi binding ta có được id = 2. Chú ý là url không phân biệt hoa thường. Nhập vào /product?idSanPham=2 cũng được
- Binding đối tượng:
+ Ví dụ 1: `public void OnGet(int? id, Product product)` khi nhập url là /product/1?name= abc; thì dối tượng product có Id = 1 và Name = abc
+ Ví dụ 2: `public void OnGet(int? id, [FromQuery]Product product)` khi nhập url là /product/1?id=3&name= abc; thì dối tượng product có Id = 3 và Name = abc
+ Ví dụ 3: `public void OnGet(int? id, [Bind("Name")]Product product)` khi nhâp url là `/product/1?id=3&name=abc` thì chỉ binding vào property Name nên Id =0 , Name = abc
## Model Binding:
- Sử dụng `[BindProperty]` để chỉ ra property nào của model sẽ được binding. Sẽ tìm theo key là tên của property trên các nguồn gửi đến. Khi tìm thấy key thì nó sẽ đọc và gán trên các thuộc tính này. `Mặc định binding loại này` thì nó chỉ thực hiện binding khi ta thực hiện phương thức `Post`. Nếu không có `[Attribute]` này phía trên thuộc tính thì sẽ không binding vào thuộc tính đó được
+ Nếu muốn hỗ trợ cả method get thì `[BindProperty(SupportsGet = true)]`. Phương thức là UserId nhưng query là userid vẫn binding được, không phân biệt hoa thường
- Sử dụng InputTagHelper:
+ Dùng `<input name="UserName">` cũng được nhưng ta nên dùng InputTagHelper cho dữ liệu thống nhất `<input asp-for="UserName" />`
+ Dùng `<input asp-for="@Model.UserName" />` hay `<input asp-for="UserName" />` là như nhau
+ Khi sử dụng `<label asp-for="UserId">Id của bạn:</label>` thì nó sẽ lấy nội dung của label để đặt tên, nếu không có nội dung nó sẽ lấy tên property để hiển thị.
- Nạp partial trong Shared để validate dữ liệu mà chưa cần submit form
```cs
@section Scripts {
    @* Nạp section này vào để khi chưa submit thì nó đã validate rồi *@
    <partial name="_ValidationScriptsPartial" />
}
```