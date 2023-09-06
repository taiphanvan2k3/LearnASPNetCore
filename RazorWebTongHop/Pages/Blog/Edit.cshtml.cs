using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages_Blog
{
    /// <summary>
    /// Nếu không có Authorize thì chỉ cần nhập đúng URL là có thể truy cập được trang này
    /// Vd: http://localhost:5272/Blog/Edit?id=2 là có thể truy cập được. Nhưng khi dùng thêm
    /// Authorize thì buộc phải login mới có thể truy cập được trang
    /// </summary>
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly RazorWebTongHop.Models.DataContext _context;

        public EditModel(RazorWebTongHop.Models.DataContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article =  await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            Article = article;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
          return _context.Articles.Any(e => e.Id == id);
        }
    }
}
