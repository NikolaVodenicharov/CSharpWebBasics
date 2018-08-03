namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Services;
    using WebServer.ByTheCakeApplication.ViewModels;
    using WebServer.Server.Http;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class ShoppingController : ByTheCakeController
    {
        private const string CartItems = "cartItems";
        private const string TotalCost = "totalCost";

        private const string searchTermKey = "searchTerm";  // we have it like a private const in CakesController class

        private readonly IProductService productService;
        private readonly IUserService userService;
        private readonly IShoppingService shoppingService;

        public ShoppingController()
        {
            this.productService = new ProductService();
            this.userService = new UserService();
            this.shoppingService = new ShoppingService();
        }

        public IHttpResponse AddToCard(IHttpRequest request)
        {
            var id = int.Parse(
                request
                .UrlParameters["id"]);

            var IsProductExist = this.productService.IsExisting(id);
            if (!IsProductExist)
            {
                return new NotFoundResponse();
            }

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.ProductIds.Add(id);


            //var redirectUrl = "/search";

            //if (request.UrlParameters.ContainsKey(searchTermKey))
            //{
            //    redirectUrl = $"{redirectUrl}?{searchTermKey}={request.UrlParameters[searchTermKey]}";
            //}

            //return new RedirectResponse(redirectUrl);

            return this.SearchRedirection(request);
        }
        private RedirectResponse SearchRedirection(IHttpRequest request)
        {
            var redirectUrl = "/search";

            if (request.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={request.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.ProductIds.Any())
            {
                this.AddProductsViews(shoppingCart);
            }
            else
            {
                this.AddNoItemsMessage();
            }

            return this.FileViewResponse(@"Shopping\Cart");
        }
        private void AddProductsViews(ShoppingCart shoppingCart)
        {
            var productInCartViews = this.productService.FindProductInCart(shoppingCart.ProductIds);

            var productDiv = productInCartViews.Select(i => $"<div>{i.Name} - ${i.Price:F2}</div><br/>");
            this.ViewData[CartItems] = string.Join(string.Empty, productDiv);

            var productsPrice = productInCartViews.Sum(i => i.Price);
            this.ViewData[TotalCost] = $"{productsPrice:F2}";
        }
        private void AddNoItemsMessage()
        {
            this.ViewData[CartItems] = "No items in your cart";
            this.ViewData[TotalCost] = "0.00";
        }

        public IHttpResponse FinishOrder(IHttpRequest request)
        {
            var username = request.Session.Get<string>(SessionStore.CurrentUserKey);
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            var userId = this.userService.GetUserId(username);
            if (userId == 0)
            {
                throw new InvalidOperationException($"User {username} does not exist.");
            }

            var productIds = shoppingCart.ProductIds;
            if (!productIds.Any())
            {
                return new RedirectResponse("/");
            }

            this.shoppingService.CreateOrder(userId, productIds); 
            shoppingCart.ProductIds.Clear();

            return this.FileViewResponse(@"shopping\finish-order");
        }
    }
}
