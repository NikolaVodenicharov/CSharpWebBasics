namespace WebServer.ByTheCakeApplication.ViewModels.Products
{
    public class ProductInCartViewModel
    {
        public ProductInCartViewModel(string name, decimal price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
