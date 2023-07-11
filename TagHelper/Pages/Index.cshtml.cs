using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TagHelper.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [DisplayName("Tên người dùng")]
    public string UserName { get; set; } = "taiphanvan2403";

    [DataType(DataType.DateTime)]
    [DisplayName("Ngày sinh")]
    public DateTime Birthday { get; set; }

    [DisplayName("Giới tính")]
    public bool Gender { get; set; } = true;

    [DisplayName("Ngày tạo")]
    [DataType(DataType.Time)]
    public DateTime CreatedAt{ get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
