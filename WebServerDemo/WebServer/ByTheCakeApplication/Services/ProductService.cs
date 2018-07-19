namespace WebServer.ByTheCakeApplication.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Data;
    using WebServer.ByTheCakeApplication.Data.Models;
    using WebServer.ByTheCakeApplication.ViewModels.Products;

    public class ProductService : IProductService
    {
        public void Create(string name, decimal price, string imageUrl)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var product = new Product(name, price, imageUrl);

                db.Products.Add(product);
                db.SaveChanges();
            }
        }

        public IEnumerable<ProductListingViewModel> All(string searchTerm = null)
        {
            using (var db = new ByTheCakeDbContext())
            {
                var resultQuery = db
                    .Products
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    resultQuery = resultQuery
                        .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()));
                }


                return resultQuery
                    .Select(p => new ProductListingViewModel(
                        p.Id,
                        p.Name,
                        p.Price))
                    .ToList();
            }
        }

        public ProductDetailsViewModel FindById(int id)
        {
            using (var db = new ByTheCakeDbContext())
            {
                return db
                    .Products
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDetailsViewModel(
                        p.Name,
                        p.Price,
                        p.ImageUrl))
                    .FirstOrDefault(); 
            }
        }
    }
}
