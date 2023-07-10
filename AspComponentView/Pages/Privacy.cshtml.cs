using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspComponentView.Pages;

public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        // PageModel: trả về partial thông qua Partial, ViewComponent
        // Controller: PartialView, ViewComponent
        // return Partial("_Message");
        // return ViewComponent("ProductBox", true);
    }
}

