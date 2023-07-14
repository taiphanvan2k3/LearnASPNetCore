using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages.BlogMySelf
{
    public class DeleteModel : PageModel
    {
        private readonly DataContext _context;

        public Article Article{ get; set; }
        public DeleteModel(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Article = await _context.Articles.FindAsync(id);
            return Page();
        }

        public async Task<IActionResult> OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Article deletedArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);
            if (deletedArticle != null)
            {
                _context.Articles.Remove(deletedArticle);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Index");
        }
    }
}
