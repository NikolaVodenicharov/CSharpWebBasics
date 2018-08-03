namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Services;
    using WebServer.ByTheCakeApplication.ViewModels;
    using WebServer.ByTheCakeApplication.ViewModels.Products;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class ProductsController : ByTheCakeController
    {
        private const string PathProductsSearch = @"products\search";
        private const string PathProductsDetails = @"products\details";

        private const string SearchTermKey = "searchTerm";

        private const string HtmlShowCart = "showCart";
        private const string HtmlProducts = "products";

        private const string HtmlShowResult = "showResult";
        private const string HtmlResults = "results";

        private readonly IProductService productService;
        
        public ProductsController()
        {
            this.productService = new ProductService();
        }

        public IHttpResponse Add()
        {
            this.HideErrorDiv();
            this.ViewData[HtmlShowResult] = HtmlNone;

            return this.AddProductResponse();
        }
        public IHttpResponse Add(AddProductViewModel model)
        {
            this.HideErrorDiv();

            if (model.Name.Length < 3 ||
                model.Name.Length > 30 ||
                model.ImageUrl.Length < 3 ||
                model.ImageUrl.Length > 2000)
            {
                this.ShowErrorDiv("Product information is not valid");

                return this.AddProductResponse();
            }

            this.productService.Create(model.Name, model.Price, model.ImageUrl);

            this.ViewData["name"] = model.Name;
            this.ViewData["price"] = model.Price.ToString();
            this.ViewData["imageUrl"] = model.ImageUrl;
            this.ViewData[HtmlShowResult] = HtmlBlock;

            return this.AddProductResponse();
        }
        private IHttpResponse AddProductResponse()
        {
            return this.FileViewResponse(@"products\add");
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            this.HideInitialSearchTerm();
            this.ShowDatabaseProducts(request);
            this.ShowCartProductsCount(request);

            return this.FileViewResponse(PathProductsSearch);
        }
        private void HideInitialSearchTerm()
        {
            this.ViewData[SearchTermKey] = string.Empty;
        }
        private void ShowDatabaseProducts(IHttpRequest request)
        {
            var urlParameters = request.UrlParameters;

            var searchTerm = urlParameters.ContainsKey(SearchTermKey)
                ? urlParameters[SearchTermKey]
                : null;

            var productViewModels = productService.All(searchTerm);

            if (productViewModels.Any())
            {
                var allProducts = productViewModels
                    .Select(p => $@"<div><a href=""/products/{p.Id}"">{p.Name}</a> - ${p.Price:F2} <a href=""/shopping/add/{p.Id}?searchTerm={searchTerm}"">Order</a> </div>");

                this.ViewData[HtmlResults] = string.Join(Environment.NewLine, allProducts);
            }
            else
            {
                this.ViewData[HtmlResults] = "No cakes found";
            }
        }
        private void ShowCartProductsCount(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.ProductIds.Any())
            {
                var productsCount = shoppingCart.ProductIds.Count;
                var productSuffix = productsCount != 1 ? "products" : "product";

                this.ViewData[HtmlShowCart] = HtmlBlock;
                this.ViewData[HtmlProducts] = $"{productsCount} {productSuffix}";
            }
            else
            {
                this.ViewData[HtmlShowCart] = HtmlNone;
            }
        }

        public IHttpResponse Details(int id)
        {
            var product = this.productService.FindById(id);

            if (product == null)
            {
                return new NotFoundResponse();
            }

            this.ViewData["name"] = product.Name;
            this.ViewData["price"] = product.Price.ToString("F2");
            this.ViewData["imageUrl"] = product.ImageUrl;

            return this.FileViewResponse(PathProductsDetails);
        }
    }
}
