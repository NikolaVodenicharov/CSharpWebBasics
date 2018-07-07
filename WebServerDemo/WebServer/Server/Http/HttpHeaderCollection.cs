namespace WebServer.Server.Http
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using WebServer.Server.Common;
    using WebServer.Server.Http.Contracts;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, ICollection<HttpHeader>> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, ICollection<HttpHeader>>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));

            var headerKey = header.Key;

            if (!this.headers.ContainsKey(headerKey))
            {
                this.headers.Add(headerKey, new List<HttpHeader>());
            }

            this.headers[headerKey]
                .Add(header);
        }
        public void Add(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            var header = new HttpHeader(key, value);
            this.Add(header);
        }
        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return this.headers.ContainsKey(key);
        }
        public ICollection<HttpHeader> Get(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            if (!this.headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The given key {key} is not present in the headers collection.");
            }

            return this.headers[key];
        }

        public IEnumerator<ICollection<HttpHeader>> GetEnumerator()
            => this.headers.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => this.headers.Values.GetEnumerator();

        public override string ToString()
        {
            var sb = new StringBuilder();

            var headersCollections = this.headers.Values;

            foreach (var headersCollection in headersCollections)
            {
                foreach (var header in headersCollection)
                {
                    sb.AppendLine(header.ToString());
                }
            }

            return sb.ToString().TrimEnd();
        }       
    }
}
