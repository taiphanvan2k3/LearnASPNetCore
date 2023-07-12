using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationExample.Models;

namespace ValidationExample.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IWebHostEnvironment _env;

        public ContactModel(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Khi dùng [BindProperty] ở đây thì nó tự động binding vào danh sách thuộc
        // tính
        [BindProperty]
        public CustomerInfo CustomerInfo { get; set; }

        // Upload file
        [BindProperty]
        [DataType(DataType.Upload)]
        [DisplayName("File upload")]
        // [FileExtensions(Extensions = "jpg,png")] // lỗi
        [CheckFileExtension(Extensions = "jpg,png")]
        public IFormFile FileUpload { get; set; }

        [BindProperty]
        [CheckFileExtension(Extensions = "png")]
        public IFormFile[] FileUploads { get; set; }

        public void OnGet()
        {
        }

        public string ThongBao { get; set; }

        private void SaveFileUpload()
        {
            // Lưu file upload
            if (FileUpload != null)
            {
                // _env.WebRootPath: lấy ra đường dẫn đến thư mục wwwroot, kết quả: D:\\Web\\LearnASPNetCore\\ValidationExample\\wwwroot
                // Sau khi nối chuỗi sinh ra: D:\\Web\\LearnASPNetCore\\ValidationExample\\wwwroot\\uploads\\avatar.jpg
                var filePath = Path.Combine(_env.WebRootPath, "uploads", FileUpload.FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                FileUpload.CopyTo(fileStream);
            }

            // Nên có bước tạo thư mục uploads trong wwwroot nữa
            if (!Directory.Exists("wwwroot/uploads"))
                Directory.CreateDirectory("wwwroot/uploads");

            foreach (var f in FileUploads)
            {
                // Hoặc chỉ đơn giản là filePath: wwwroot/uploads + f.FileName là được
                // vì đọc/ghi file thì đường dẫn tính từ thư mục của project mà
                var filePath = Path.Combine(_env.WebRootPath, "uploads", f.FileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                f.CopyTo(fileStream);
            }
        }

        public void OnPost()
        {
            // Muốn biết dữ liệu tất cả dữ liệu có phù hợp hay không?
            if (ModelState.IsValid)
            {
                ThongBao = "Dữ liệu gửi đến phù hợp";
                SaveFileUpload();
            }
            else
            {
                ThongBao = "Dữ liệu gửi đến không phù hợp";
            }
        }
    }
}
