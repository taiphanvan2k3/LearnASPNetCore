using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPNet08.Pages;

public class IndexModel : PageModel
{
    public string Abc{ get; set; }
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        // Khi truy cập trang bằng method get thì sẽ gọi phương thức này
        Abc = "Giá trị được khởi tạo từ PageModel";
    }
}
