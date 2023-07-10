using AspComponentView.Models;
using AspComponentView.Services;
using Microsoft.AspNetCore.Mvc;

namespace Components.ProductBox
{
    // [ViewComponent]
    public class ProductBox : ViewComponent
    {
        private readonly ProductListService _productListService;
        public ProductBox(ProductListService productListService)
        {
            _productListService = productListService;
        }
        
        // Để nó được coi là Component thì nó phải có Invoke() hoặc InvokeAsync()
        // Thiết lập lớp này có attribute [ViewComponent]
        public IViewComponentResult Invoke(bool sapXepTang = true)
        {
            // C1: Mặc định thi hành Default.cshtml
            // return View();

            // C2: sử dụng view khác
            // return View("Default1");

            // C3: truyền dữ liệu qua View (qua View default)
            // return View<string>("chuỗi string truyền qua view nè.");
            List<Product> temp = null;
            if (!sapXepTang)
            {
                temp = _productListService.products.OrderByDescending(p => p.Price).ToList();
            }
            else
            {
                temp = _productListService.products.OrderBy(p => p.Price).ToList();
            }

            // truyền model qua view Default1.cshtml
            return View<List<Product>>("Default1", temp);
        }
    }
}