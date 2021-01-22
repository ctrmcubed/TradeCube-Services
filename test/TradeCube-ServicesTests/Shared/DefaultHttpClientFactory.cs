using System.Net.Http;

namespace TradeCube_ServicesTests.Shared
{
    public sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) =>
            new HttpClient();
    }
}