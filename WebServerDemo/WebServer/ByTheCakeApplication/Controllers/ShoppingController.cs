namespace WebServer.ByTheCakeApplication.Controllers
{
    using System.Linq;

    using WebServer.ByTheCakeApplication.Data;
    using WebServer.ByTheCakeApplication.Infrastructure;
    using WebServer.ByTheCakeApplication.Models;
    using WebServer.Server.Http.Contracts;
    using WebServer.Server.Http.Response;

    public class ShoppingController : Controller
    {
        private const string CartItems = "cartItems";
        private const string NoItemsMessage = "No items in your cart";
        private const string TotalCost = "totalCost";

        private readonly CakesData cakesData;

        public ShoppingController()
        {
            this.cakesData = new CakesData();
        }

        public IHttpResponse AddToCard(IHttpRequest request)
        {
            var id = int.Parse(
                request
                .UrlParameters["id"]);

            var cake = this.cakesData.Find(id);

            if (cake == null)
            {
                return new NotFoundResponse();
            }

            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.Orders.Add(cake);

            var redirectUrl = "/search";
            const string searchTermKey = "searchTerm";  // we have it like a private const in CakesController class

            if (request.UrlParameters.ContainsKey(searchTermKey))
            {
                redirectUrl = $"{redirectUrl}?{searchTermKey}={request.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }

        public IHttpResponse ShowCart(IHttpRequest request)
        {
            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.Orders.Any())
            {
                this.ViewData[CartItems] = NoItemsMessage;
                this.ViewData[TotalCost] = "0.00";
            }
            else
            {
                var items = shoppingCart
                    .Orders
                    .Select(i => $"<div>{i.Name} - ${i.Price:F2}</div><br/>");

                this.ViewData[CartItems] = string.Join(string.Empty, items);

                var totalPrice = shoppingCart
                    .Orders
                    .Sum(i => i.Price);

                this.ViewData[TotalCost] = $"{totalPrice:F2}";
            }

            return this.FileViewResponse(@"Shopping\Cart");
        }

        public IHttpResponse FinishOrder(IHttpRequest request)
        {
            request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey).Orders.Clear();  // we dont save the ordered products

            return this.FileViewResponse(@"shopping\finish-order");
        }
    }
}
