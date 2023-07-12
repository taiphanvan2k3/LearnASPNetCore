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

        public void ReadData()
        {
            // var data = this.Request.Form["id"]
            // var data = this.Request.Headers["id"]
            // var data = this.Request.Form["id"]
            // var data = this.Request.RouteValues["id"]

            var idRouteValue = this.Request.RouteValues["id"];
            if (idRouteValue != null)
            {
                // vd /product/{id: int?} nên id là route values
                System.Console.WriteLine("id route value: " + idRouteValue.ToString());
            }

            var idQuery = this.Request.Query["id"];
            if (!string.IsNullOrEmpty(idQuery))
            {
                // url: vd /product?id=2
                // hoặc url: /product/1?id=2 thì vừa lấy ra được route values, vừa lấy ra được query
                System.Console.WriteLine("Id query: " + idQuery.ToString());
            }

            // Đọc header:
            var header = this.Request.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(header))
            {
                // url: vd /product?id=2
                // hoặc url: /product/1?id=2 thì vừa lấy ra được route values, vừa lấy ra được query
                System.Console.WriteLine("User-Agent: " + header.ToString());
            }
        }

        private void ReadDataModel(Product product)
        {
            System.Console.WriteLine($"Id: {product.Id}");
            System.Console.WriteLine($"Name: {product.Name}");
        }

        public void OnGet(int? id, [Bind("Name")]Product product)
        {
            // ReadData();
            ReadDataModel(product);

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