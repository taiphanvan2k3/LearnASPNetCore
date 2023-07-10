using AspComponentView.Components.MessagePage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspComponentView.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnPost()
    {
        string username = this.Request.Form["username"];
        var message = new Message()
        {
            Title = "Thông báo",
            HtmlContent = $"Cảm ơn {username} đã gửi thông báo",
            // urlRedirect = "/Index",
            urlRedirect = Url.Page("Index", new { area = "User" }),
            secondWait = 5
        };

        // Trả về nội dung của component
        return ViewComponent("MessagePage", message);
    }

    public void OnGet()
    {

    }
}
