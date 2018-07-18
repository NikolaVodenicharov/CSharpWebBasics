namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Enums;
    using Http.Contracts;
    using WebServer.Server.Common;
    using WebServer.Server.Exceptions;

    public class HttpRequest : IHttpRequest
    {
        private const string BadRequestExceptionMessage = "Request is not valid";

        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.UrlParameters = new Dictionary<string, string>();

            this.ParseRequest(requestString);
        }

        public HttpRequestMethod RequestMethod { get; private set; }
        public string Url { get; private set; }
        public string Path { get; private set; }
        public IHttpHeaderCollection Headers { get; private set; }
        public IHttpCookieCollection Cookies { get; private set; }
        public IHttpSession Session { get; set; }
        public IDictionary<string, string> UrlParameters { get; private set; }
        public void AddUrlParameter(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }
        public IDictionary<string, string> FormData { get; private set; }

        private void ParseRequest(string requestString)
        {
            var lines = requestString.Split(Environment.NewLine);

            if (!lines.Any())
            {
                throw new BadRequestException(BadRequestExceptionMessage);
            }

            var firstLine = lines[0].Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            if (firstLine.Length != 3 ||
                firstLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException(BadRequestExceptionMessage);
            }

            this.RequestMethod = this.ParseRequestMethod(firstLine[0]);
            this.Url = firstLine[1];
            this.Path = this.ParsePath(Url);

            this.ParseHeaders(lines);
            this.ParseCookies();
            this.SetSession();
            this.ParseUrlParameters();
            this.ParseFormData(lines.Last());
        }
        private HttpRequestMethod ParseRequestMethod(string requestMethod)
        {
            try
            {
                return Enum.Parse<HttpRequestMethod>(requestMethod, true);
            }
            catch (Exception)
            {
                throw new BadRequestException(BadRequestExceptionMessage);
            }
        }
        private string ParsePath(string url)
        {
            return url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }
        private void ParseHeaders(string[] lines)
        {
            int endIndex = Array.IndexOf(lines, String.Empty);

            for (int i = 1; i < endIndex; i++)
            {
                string[] headerArgs = lines[i].Split(
                    new[] { ": " }, 
                    StringSplitOptions.RemoveEmptyEntries);

                if (headerArgs.Length != 2)
                {
                    throw new BadRequestException(BadRequestExceptionMessage);
                }

                string key = headerArgs[0];
                string value = headerArgs[1];
                this.Headers.Add(new HttpHeader(key, value));
            }

            if (!this.Headers.ContainsKey(HttpHeader.Host))
            {
                throw new BadRequestException(BadRequestExceptionMessage);
            }
        }
        private void ParseCookies()
        {
            if (this.Headers.ContainsKey(HttpHeader.Cookie))
            {
                var allCookies = this.Headers.Get(HttpHeader.Cookie);

                foreach (var cookie in allCookies)
                {
                    if (!cookie.Value.Contains('='))
                    {
                        return;
                    }

                    var cookieParts = cookie
                        .Value
                        .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    if (!cookieParts.Any())
                    {
                        continue;
                    }

                    foreach (var cookiePart in cookieParts)
                    {
                        var cookieKeyValueParts = cookiePart
                            .Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKeyValueParts.Length != 2)
                        {
                            continue;
                        }

                        var key = cookieKeyValueParts[0].Trim();
                        var value = cookieKeyValueParts[1].Trim();

                        this.Cookies.Add(new HttpCookie(key, value, false));
                    }
                }
            }
        }
        private void SetSession()
        {
            // Cokie: SessionID=value;
            if (this.Cookies.Contains(SessionStore.SessionCookieKey))
            {
                var cookie = this.Cookies.Get(SessionStore.SessionCookieKey);
                var sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }
        private void ParseUrlParameters()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }

            var query = this.Url
                .Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Last();

            this.ParseQuery(query, this.UrlParameters);
        }
        private void ParseFormData(string formDataLine)
        {
            if (this.RequestMethod != HttpRequestMethod.Post)
            {
                return;
            }

            this.ParseQuery(formDataLine, this.FormData);
        }
        private void ParseQuery(string queryString, IDictionary<string, string> dict)
        {
            if (!queryString.Contains('='))
            {
                return;
            }

            var queryPairs = queryString
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryPair in queryPairs)
            {
                var pair = queryPair
                    .Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (pair.Length != 2)
                {
                    return;
                }

                var key = WebUtility.UrlDecode(pair[0]);
                var value = WebUtility.UrlDecode(pair[1]);

                dict.Add(key, value);
            }
        }
    }
}
