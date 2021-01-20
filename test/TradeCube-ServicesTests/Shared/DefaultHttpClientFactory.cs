using System;
using System.Net.Http;

namespace TradeCube_ServicesTests.Shared
{
    public sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        private static readonly Lazy<HttpClient> HttpClientLazy =
            new Lazy<HttpClient>(() => new HttpClient());

        public HttpClient CreateClient(string name) =>
            HttpClientLazy.Value;
    }
}