# Bootstrap:
- Class display-1 -> 4: cỡ chữ sẽ giảm dần
# Cú pháp razor page
- Để viết các kí tự đặc biệt trong cshtml ta sẽ viết trong @(...)
vd `@("<h1>Xin chào các bạn</h1>")`. Nội dung trong " " như nào thì sẽ được hiển thị ra như đó vì nó đã bị encode rồi. Cách này khá giúp ích nếu muốn in ra kí tự đặc biệt vd @,.. thì dùng `@("@")` là được
- Nếu muốn không bị encode tức là khi truyền vào `"<h1>Xin chào các bạn</h1>"` thì sẽ hiển thị chữ đậm thì ta dùng `@Html.Raw("<h1>Xin chào các bạn</h1>")`;
- Thẻ <text> sẽ xuất ra nội dung nhưng không nằm trong thẻ nào
- Để khai báo namespace của file cshtml thì ta dùng `@namespace <tên namespace>`
- Để using 1 namespace thì dùng `@using <tên namespace>`
# Nhắc lại: 
- Nếu trang razor page chỉ có chỉ thị @page thì mặc định EndpointRoutingMiddleware sẽ tọa ra 1 endpoint tương ứng với tên của trang razor
- Tại Area nếu muốn dùng Layout thì cũng phải tạo file Layout thì mới có Layout để dùng
- Trong 1 file cshtml, nếu sử dụng AnchorTagHelper asp-page mà không chỉ ra asp-area thì nó mặc định sử dụng file cshtml chỉ ra trong asp-for cùng cấp với file cshtml hiện tại. Nếu muốn href đến các file không nằm trong 
- Các file cshtml bên `trong thư mục con` của `/Pages` `không thể href đến các file cshtml` nằm `trực tiếp trong thư mục Pages`, vì lúc này ta `không thể chỉ rõ được asp-area của các file cshtml` đó là gì, mà `nếu không chỉ rõ thì nó sẽ đi kiếm file cshtml đó nằm cùng thư mục với file hiện tại` => `Không thể tự sinh ra href trong html`. Lúc này chỉ còn phương án là tự viết href  
Cụ thể ở đây khi truy cập đến các trang cshtml trong `/Pages/Category` thì `không thể dùng navigation` ở header được nữa vì `href không được sinh ra`