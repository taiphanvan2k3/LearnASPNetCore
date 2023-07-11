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