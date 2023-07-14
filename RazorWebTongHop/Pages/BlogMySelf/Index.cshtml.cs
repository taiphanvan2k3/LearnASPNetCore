using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages.BlogMySelf
{
    public class IndexModel : PageModel
    {
        private readonly DataContext _context;

        public IndexModel(DataContext context)
        {
            _context = context;
        }

        public async Task OnGet(string SearchString)
        {
            var query = _context.Articles.AsQueryable();
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(a => a.Title.Contains(SearchString));
            }

            var articles = await query.OrderByDescending(a => a.CreateAt)
                                         .ToListAsync();

            ViewData["articles"] = articles;
        }
    }
}
