using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PageModelExample.Models;
using PageModelExample.Services;

namespace PageModelExample.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly ILogger<ProductPageModel> _logger;
        public ProductService _productService { get; set; }

        public ProductPageModel(ILogger<ProductPageModel> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // Các handler bắt đầu là OnGet, OnPost, OnGet... trả về void hoặc IActionResult
        /* public void OnGet()
        {
            // Phương thức này sẽ được truy cập khi gọi phương thức Get đến trang

            var idRoute = this.Request.RouteValues["id"];
            if (idRoute != null)
            {
                int id = int.Parse(idRoute.ToString());
                ViewData["Title"] = $"Sản phẩm Id = {id}";
            }
            else
            {
                ViewData["Title"] = "Danh sách sản phẩm";
            }
        } */

        public Product Product { get; set; }

        public void OnGet(int? id)
        {
            // url truy cập: /product/2 hoặc /product/
            // Tìm trên route values nếu có thì nó tự động convert thành id
            if (id != null)
            {
                ViewData["Title"] = $"Sản phẩm Id = {id}";
                Product = _productService.GetProductById(id.Value);
            }
            else
            {
                ViewData["Title"] = "Danh sách sản phẩm";
            }
        }

        // /product/{id:int?}?handler=lastproduct
        public IActionResult OnGetLastProduct()
        {
            ViewData["title"] = "Sản phẩm cuối";
            Product = _productService.GetAllProducts().LastOrDefault();
            if (Product != null)
            {
                // Trả về nội dung của trang hiện tại
                return Page();
            }
            return Content("Không tìm thấy sản phẩm");
        }

        public IActionResult OnGetRemoveAll()
        {
            _productService.GetAllProducts().Clear();
            // return RedirectToPage("ProductPage");

            // return Page là đủ
            return Page();
        }

        public IActionResult OnGetInitProducts()
        {
            _productService.InitProducts();
            // return RedirectToPage("ProductPage");

            // return Page là đủ
            return Page();
        }

        public IActionResult OnPostDeleteProduct(int? id)
        {
            if (id != null)
            {
                Product = _productService.GetProductById(id.Value);
                if (Product != null)
                {
                    _productService.GetAllProducts().Remove(Product);
                }
            }

            // Cần tạo HTTP request mới sau khi thực hiện method POST
            return RedirectToPage("ProductPage");
        }
    }
}