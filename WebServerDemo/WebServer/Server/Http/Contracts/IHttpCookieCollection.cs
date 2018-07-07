namespace WebServer.Server.Http.Contracts
{
    using System.Collections.Generic;

    public interface IHttpCookieCollection : IEnumerable<HttpCookie>
    {
        void Add(HttpCookie cookie);
        void Add(string key, string value);
        bool Contains(string key);
        HttpCookie Get(string key);
    }
}
