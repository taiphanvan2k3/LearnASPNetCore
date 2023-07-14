using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages.BlogMySelf
{
    public class IndexModel : PageModel
    {
        private readonly DataContext _context;

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public int ITEMS_PER_PAGE = 8;

        [BindProperty(SupportsGet = true, Name = "p")]

        // Mỗi lần binding là nó reset lại giá trị mặc định của property, 
        // Cho nó 1 giá trị mặc định là 1 vì khi submit form OnGet không submit dữ liệu cho
        // nó, dẫn đến nó bằng 0. 
        public int CurrentPage { get; set; } = 1;

        public int CountPages { get; set; }

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

            // Tính toán tổng số record sau khi query và trước khi Skip,Take
            int totalArticle = (await query.ToListAsync()).Count;

            var articles = await query.Skip((CurrentPage - 1) * ITEMS_PER_PAGE)
                                    .Take(ITEMS_PER_PAGE)
                                    .OrderByDescending(a => a.CreateAt)
                                    .ToListAsync();

            CountPages = (int)Math.Ceiling(totalArticle * 1.0 / ITEMS_PER_PAGE);

            if (CurrentPage < 1)
                CurrentPage = 1;
            else if (CurrentPage > CountPages)
                CurrentPage = CountPages;

            ViewData["articles"] = articles;
        }
    }
}
