namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Data;
    using WebServer.ByTheCakeApplication.Infrastructure;
    using WebServer.ByTheCakeApplication.ViewModels;
    using WebServer.Server.Http.Contracts;

    public class CakesController : Controller
    {
        private const string CakesAdd = @"Cakes\Add";
        private const string CakesSearch = @"Cakes\Search";
        private const string SearchTermKey = "searchTerm";

        private const string ShowCart = "showCart";
        private const string Products = "products";

        private const string ShowResult = "showResult";
        private const string Results = "results";

        private const string Block = "block";
        private const string None = "none";

        private readonly CakesData cakesData;
        
        public CakesController()
        {
            this.cakesData = new CakesData();
        }

        public IHttpResponse Add()
        {
            this.ViewData[ShowResult] = None;

            return this.FileViewResponse(CakesAdd);
        }

        public IHttpResponse Add(string name, string price)
        {
            this.cakesData.Add(name, price);

            this.ViewData["name"] = name;
            this.ViewData["price"] = price;
            this.ViewData[ShowResult] = Block;

            return this.FileViewResponse(CakesAdd);
        }

        public IHttpResponse Search(IHttpRequest request)
        {
            var urlParameters = request.UrlParameters;

            this.ViewData[Results] = String.Empty;
            this.ViewData[SearchTermKey] = string.Empty;

            if (urlParameters.ContainsKey(SearchTermKey))
            {
                var searchTerm = urlParameters[SearchTermKey];

                this.ViewData[SearchTermKey] = searchTerm;

                var savedCakesDivs = this.cakesData
                    .All()
                    .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(c => $@"<div>{c.Name} - ${c.Price:F2} <a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}"">Order</a> </div>");

                this.ViewData[Results] = string.Join(Environment.NewLine, savedCakesDivs);
            }

            this.ViewData[ShowCart] = None;


            var shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.Orders.Any())
            {
                var productsCount = shoppingCart.Orders.Count;
                var productSuffix = productsCount != 1 ? "products" : "product";
                
                this.ViewData[ShowCart] = Block;
                this.ViewData[Products] = $"{productsCount} {productSuffix}";
            }

            return this.FileViewResponse(CakesSearch);
        }
    }
}
