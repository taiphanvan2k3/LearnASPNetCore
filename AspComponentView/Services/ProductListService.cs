using AspComponentView.Models;

namespace AspComponentView.Services
{
    public class ProductListService
    {
        public List<Product> products { get; set; } = new List<Product>()
        {
            new Product(){ Name = "SP 1", Description = "Mô tả cho SP 1", Price = 1000},
            new Product(){ Name = "SP 2", Description = "Mô tả cho SP 2", Price = 2000},
            new Product(){ Name = "SP 3", Description = "Mô tả cho SP 3", Price = 3000},
            new Product(){ Name = "SP 4", Description = "Mô tả cho SP 4", Price = 4000},
        };
    }
}