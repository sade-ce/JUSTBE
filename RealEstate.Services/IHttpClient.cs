using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;


namespace RealEstate.Services
{
    public interface IHttpClient
    {
        Uri BaseAddress { get; set; }

        HttpRequestHeaders DefaultRequestHeaders { get; }

        Task<HttpResponseMessage> GetAsync(Uri uri);
    }

    /// <summary>
    /// Wraps an HttpClient to allow mocking in unit tests.
    /// </summary>
    public class HttpClientWrapper : IHttpClient
    {
        private HttpClient wrappedClient;

        public HttpClientWrapper()
        {
            this.wrappedClient = new HttpClient();
        }

        public HttpClientWrapper(HttpClient clientToWrap)
        {
            this.wrappedClient = clientToWrap;
        }

        public Uri BaseAddress
        {
            get { return wrappedClient.BaseAddress; }
            set { wrappedClient.BaseAddress = value; }
        }

        public HttpRequestHeaders DefaultRequestHeaders
        {
            get { return wrappedClient.DefaultRequestHeaders; }
        }

        public Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return wrappedClient.GetAsync(uri);
        }

    }
}
