using PageModelExample.Models;

namespace PageModelExample.Services
{
    public class ProductService
    {
        private List<Product> products = new List<Product>();

        public ProductService()
        {
            InitProducts();
        }

        public void InitProducts()
        {
            products.Clear();

            products.Add(new Product()
            {
                Id = 1,
                Name = "IPhone",
                Price = 900,
                Description = "IPhone X"
            });

            products.Add(new Product()
            {
                Id = 2,
                Name = "Samsung",
                Price = 1000,
                Description = "Samsung S23"
            });

            products.Add(new Product()
            {
                Id = 3,
                Name = "Nokia",
                Price = 800,
                Description = "Điện thoại Nokia"
            });
        }

        public Product GetProductById(int id)
        {
            return products.Where(p => p.Id == id).FirstOrDefault();
        }

        public List<Product> GetAllProducts() => products;
    }
}