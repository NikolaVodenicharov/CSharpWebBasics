namespace WebServer.Server.Http.Contracts
{
    using System.Collections.Generic;
    using WebServer.Server.Enums;

    public interface IHttpRequest
    {
        IDictionary<string, string> FormData { get; }
        IHttpHeaderCollection Headers { get; }
        IHttpCookieCollection Cookies { get; }
        string Path { get; }
        HttpRequestMethod RequestMethod { get; }
        string Url { get; }
        IDictionary<string, string> UrlParameters { get; }
        IHttpSession Session { get; set; }

        void AddUrlParameter(string key, string value);
    }
}
