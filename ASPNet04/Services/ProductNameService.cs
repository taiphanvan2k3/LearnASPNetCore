namespace ASPNet04.Services
{
    public class ProductNameService
    {
        private List<string> _names { get; set; }

        public ProductNameService()
        {
            _names = new List<string>()
            {
                "IPhone X",
                "Samsung Galaxy S23",
                "Laptop Thinkpad P15s"
            };
        }

        public List<string> GetNames() => _names;
    }
}