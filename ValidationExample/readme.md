- Nhắc lại syntax tạo trang Razor page bằng terminal:
`dotnet new page -o Pages -n Contact --namespace <tên namespace>`
# Validation:
- Emmet tạo nhanh bộ 3 label,input, validation: asp-form-group-bootstarp
- Thiết lập độ dài chuỗi nằm trong phạm vi nào đó: `[StringLength(20)]`, nếu ràng buộc min length thì `[StringLength(20, MinimumLength = 3)]`
- Các attribute thông dụng:
+ Required
+ StringLength: chiều dài chuỗi (tối thiểu, tối đa)
+ Range: số nằm trong phạm vi nào đó
+ RegularExpression
+ EmailAddress
+ FileExtension: những phần mở rộng phù hợp
+ MaxLength: chiều dài tối đa của mảng, chuỗi
+ MinLength: chiều dài tối thiểu của mảng, chuỗi
+ Phone
+ CustomValidation: tự làm, phải kế thừa từ ValidationAttribute và nạp chồng IsValid, phương thức này tự custom lại
- Trong PageModel hay Controller sẽ có thuộc tính ModelState để kiểm tra dữ liệu binding có đúng hết không?
- Ta có thể can thiệp vào quá trình chuyển đổi dữ liệu khi binding. Để làm được như vậy thì ta phải tạo ra các Binder riêng, các lớp này kế thừa từ IModelBinder
+ Để thiết lập binder cho 1 property nào đó: `[ModelBinder(BinderType = typeof(UserNameBinding))]`
- Upload file: để upload file thì form phải có thuộc tính `enctype="multipart/form-data"`
+ Khi dùng InputTagHelper cho thuộc tính IFormFile thì nó sinh ra thẻ input với `type="file"`
+ Khi sử dụng attribute `[FileExtensions(Extensions = "jpg,png")]` thì dù upload file đúng phần mở rộng nhưng vẫn không được do source code của attribute đó có vấn đề
là nó convert `object value`thành string trong khi file upload lại ở dạng IFormFile
+ Đã custom lại code của thư viện ở class CheckFileExtension, tuy vẫn có thể thiết lập bộ lọc khi mở hộp thoại chọnthông qua thuộc tính `accept của input type="file"` nhưng người dùng vẫn cố ý chọn file loại khác thì ta ngăn chặn được thông qua `Attribute`