using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages.BlogMySelf
{
    public class CreateModel : PageModel
    {
        private readonly DataContext _context;

        [BindProperty]
        public Article Article { get; set; }

        public CreateModel(DataContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Articles.Add(Article);
                // Hoặc _context.Add(Article); cũng được rồi
                _context.SaveChanges();
                return RedirectToPage("./Index");
            }   
            else
            {
                return Page();
            }
        }
    }
}
