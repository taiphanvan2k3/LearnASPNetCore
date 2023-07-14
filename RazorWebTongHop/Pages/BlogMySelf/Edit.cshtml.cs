using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages.BlogMySelf
{
    public class EditModel : PageModel
    {
        private readonly DataContext _context;

        [BindProperty]
        public Article Article { get; set; }

        public EditModel(DataContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
            // Do thiết lập @page có route value là id nên nó sẽ tự động binding vào
            // id lúc này lấy từ nguồn dữ liệu ở route values
            Article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (Article == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _context.Attach(Article).State = EntityState.Modified;
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
