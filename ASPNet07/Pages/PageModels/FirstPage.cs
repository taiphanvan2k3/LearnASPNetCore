using Microsoft.AspNetCore.Mvc.RazorPages;

// Tách riêng ra namespace để chứa các file xử lí logic cho các file cshtml
namespace ASPNet07.Pages.PageModels
{
    // Phải kế thừa từ PageModel
    public class FirstPageModel : PageModel
    {
        public string title { get; set; } = "Đây là trang razor page đầu tiên";
        public int number = 10;

        public string Welcome(string name)
        {
            return $"Chào mừng {name} đến với website";
        }

        // Các handler
        public void OnGet()
        {
            // Khi truy vấn đến trang thì nó tự động gọi phương thức này đầu tiên
            // sau đó nó mới chạy từ đầu file đến
            System.Console.WriteLine("Truy van bang phuong thuc Get");
            ViewData["mydata"] = $"Tự học Razor page 08/07/2023";
            // Thread.Sleep(1000);
            // ViewData này sẽ bị ghi đè nếu ta khi biên dịch file cshtml thấy khởi tạo lại, Thread.Sleep sẽ thấy
        }

        // Khi truy cập url có dạng: url?handler=xyz
        public void OnGetXyz()
        {
            System.Console.WriteLine("Truy van bang phuong thuc onGetXyz");
            ViewData["mydata"] = $"Tự học Razor page 08/07/2023 XYZ";
        }

        // Tương tự ta có OnPost(), OnPostAbc()
    }
}