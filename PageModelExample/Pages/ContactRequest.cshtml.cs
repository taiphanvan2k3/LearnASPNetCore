using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PageModelExample.Models;

namespace PageModelExample.Pages
{
    public class ContactRequestModel : PageModel
    {
        [BindProperty]
        public UserContact UserContact { get; set; } = new UserContact();

        private readonly ILogger<ContactRequestModel> _logger;
        public ContactRequestModel(ILogger<ContactRequestModel> logger)
        {
            _logger = logger;
            _logger.LogInformation("Init contact...");
        }

        public double Sum(double a, double b) => a + b;

        public void OnPost()
        {
            System.Console.WriteLine(this.UserContact.UserId);
            System.Console.WriteLine(this.UserContact.UserName);
            System.Console.WriteLine(this.UserContact.Email);
        }
    }
}