using Newtonsoft.Json;

namespace ASPNet05.ProductManage
{
    public static class ProductController
    {
        private static List<Product> products;

        public static string BuyMoreProduct(HttpContext context)
        {
            if (products == null)
                products = new List<Product>();

            string bought_products = context.Session.GetString("bought-products");
            if (bought_products == null)
            {
                products = new List<Product>();
            }
            else
            {
                products = JsonConvert.DeserializeObject<List<Product>>(bought_products);
            }

            var product = new Product
            {
                Id = products.Count + 1,
                Name = $"Cellphone #{products.Count}",
                BuyAt = DateTime.Now
            };

            products.Add(product);
            string json = JsonConvert.SerializeObject(products);

            // Thường dùng SetString để lưu trữ dữ liệu của các đối tượng khi chuyển về json
            context.Session.SetString("bought-products", json);
            return json;
        }

        public static string ViewProducts(HttpContext context)
        {
            return context.Session.GetString("bought-products") ?? "Empty list";
        }
    }
}