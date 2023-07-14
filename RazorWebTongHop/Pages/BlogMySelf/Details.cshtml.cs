using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages.BlogMySelf
{
    public class DetailsModel : PageModel
    {
        private readonly DataContext _context;

        public Article Article { get; set; }

        public DetailsModel(DataContext context)
        {
            _context = context;
        }
        public void OnGet(int id)
        {
            System.Console.WriteLine(id);
            Article = _context.Articles.FirstOrDefault(a => a.Id == id);
        }
    }
}
