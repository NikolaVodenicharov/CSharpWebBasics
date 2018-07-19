namespace WebServer.ByTheCakeApplication.ViewModels.Products
{
    using System;

    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel(string name, decimal price, string imageUrl)
        {
            this.Name = name;
            this.Price = price;
            this.ImageUrl = imageUrl;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
