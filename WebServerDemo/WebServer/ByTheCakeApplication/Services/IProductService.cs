namespace WebServer.ByTheCakeApplication.Services
{
    using System.Collections.Generic;

    using WebServer.ByTheCakeApplication.ViewModels.Products;

    public interface IProductService
    {
        void Create(string name, decimal price, string imageUrl);

        IEnumerable<ProductListingViewModel> All(string searchTherm = null);

        ProductDetailsViewModel FindById(int id);

        bool IsExisting(int id);

        IEnumerable<ProductInCartViewModel> FindProductInCart(IEnumerable<int> productIds);
    }
}
