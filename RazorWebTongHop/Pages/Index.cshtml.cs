using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorWebTongHop.Models;

namespace RazorWebTongHop.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _context;

    public IndexModel(ILogger<IndexModel> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        var articles = _context.Articles.OrderByDescending(a => a.CreateAt)
                                        .ToList();
        ViewData["articles"] = articles;
    }
}
