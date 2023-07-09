# Tìm hiểu về Layout
- Thông thường các trang sử dụng cùng 1 Layout để dùng chung header, footer, menu,...
- Ta có thể tạo ra nhiều Layout để thiết lập các trang sử dụng Layout đó. Khi mỗi trang cshtml được truy cập thì nó tự động chèn _ViewStart.cshtml vào phần đầu. Lúc này các trang đó được thiết lập sẵn Layout mặc định là _Layout.cshtml. Tuy nhiên nếu muốn ta có thể tạo ra Layout khác rồi thiết lập vào thuộc tính Layout
+ File layout tạo ra phải được để ở một trong các thư mục sau:
```
/Pages/<tên file layout>
/Pages/Shared/<tên file layout>
/Views/Shared/<tên file layout>
```
+ Đặt tên có sử dụng _ ở đầu để EndpointRoutingMiddleware không sử dụng file có _ ở đầu để làm endpoints
# Tạo section
```html
<div class="bg-success">Footer
    @await RenderSectionAsync("NoiDungFooter", required: false)
</div>
```
+ Khi muốn chèn nội dung của section ở đâu đó thì ta dùng RenderSectionAsync hoặc RenderSection. Người ta thường required: false để không bắt buộc tạo section
- Có thể tạo các Layout lồng nhau:
+ Chỉ Layout cuối cùng mới viết cấu trúc `<head> <body>`, các layout con chỉ viết code cshtml giống với các trang cshtml. Vì nội dung các layout con này sẽ được render vào phần body của master nên các layout con mới không viết cấu trúc `<head> <body>`
+ Các layout con cần thực hiện việc `chuyển tiếp` các section đến master bằng cách cũng `tạo ra các section` và các section này của layout con chỉ làm nhiệm vụ render lại  section của trang  
vd:
```cs
@section NoiDungFooter{
    @await RenderSectionAsync("NoiDungFooter", required: false)
}
```
- href đến các file static không cần ~ ở đầu cũng được:
```html
@section Styles{
    <link rel="stylesheet" href="~/css/Post/style.css">
}

Hay

@section Styles{
   <link rel="stylesheet" href="/css/Post/style.css">
}
đều như nhau
```
# Bootstrap:
- pb-*: padding bottom (có giá trị pb-1 -> pb-5)
- Tạo thanh navbar collapse bằng Bootstrap:
```html
<nav class="navbar navbar-expand-md navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
```
+ expand menu sẽ xuất hiện khi ở màn hình trung bình. Ban đầu với các màn hình lớn sẽ không có nhưng khi co màn hình lại hoặc ở các thiết bị có màn hình nhỏ thì mới bắt đầu xuất hiện. Thêm nữa cần chỉnh sửa lại class của thẻ ul để collapse button hoạt động chính xác `<ul class="navbar-nav">`
- Có sự khác biệt giữa `<div class="container-fluid">` và `<div class="container">`. `<div class="container-fluid">` sẽ khiến chiều rộng container rộng hơn