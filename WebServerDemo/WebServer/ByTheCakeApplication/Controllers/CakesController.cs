namespace WebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using WebServer.ByTheCakeApplication.Infrastructure;
    using WebServer.ByTheCakeApplication.Models;
    using WebServer.Server.Http.Contracts;

    public class CakesController : Controller
    {
        private const string DatabasePath = @"..\..\..\ByTheCakeApplication\Data\database.csv";
        private static List<Cake> cakes = new List<Cake>();

        public IHttpResponse Add()
        {
            var result = this.FileViewResponse(
                @"Cakes\Add",
                new Dictionary<string, string>
                    {
                        ["showResult"] = "none"
                    });

            return result;
        }

        public IHttpResponse Add(string name, string price)
        {
            var cake = new Cake(name, decimal.Parse(price));
            cakes.Add(cake);

            using (var streamWriter = new StreamWriter(DatabasePath, true))
            {
                streamWriter.WriteLine($"{name},{price}");
            }

            var result = this.FileViewResponse(
                @"cakes\add",
                new Dictionary<string, string>
                    {
                        ["name"] = name,
                        ["price"] = price,
                        ["showResult"] = "block"
                    });

            return result;
        }

        public IHttpResponse Search(IDictionary<string, string> urlParameters)
        {
            const string searchTermKey = "searchTerm";

            var results = String.Empty;

            if (urlParameters.ContainsKey(searchTermKey))
            {
                var searchTerm = urlParameters[searchTermKey];

                var savedCakesDivs = File
                    .ReadAllLines(DatabasePath)
                    .Where(l => l.Contains(','))
                    .Select(l => l.Split(','))
                    .Select(l => new Cake(l[0], decimal.Parse(l[1])))
                    .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(c => $"<div>{c.Name} - ${c.Price}</div>");

                results = string.Join(Environment.NewLine, savedCakesDivs);
            }

            var response = this.FileViewResponse(
                @"Cakes\Search", 
                new Dictionary<string, string>
                    {
                        ["results"] = results
                    });

            return response;
        }
    }
}
