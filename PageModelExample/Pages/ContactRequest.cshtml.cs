using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PageModelExample.Pages
{
    public class ContactRequestModel : PageModel
    {
        private readonly ILogger<ContactRequestModel> _logger;
        public ContactRequestModel(ILogger<ContactRequestModel> logger)
        {
            _logger = logger;
            _logger.LogInformation("Init contact...");
        }

        public string UserId { get; set; } = "taiphanvan2403";

        public double Sum(double a, double b) => a + b;
    }
}