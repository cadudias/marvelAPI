using Marvel.Api;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Marvel.Api
{
    public class CustomHttpClient : IHttpClient
    {
        private readonly IHttpClientFactory clientFactory;

        public CustomHttpClient(IHttpClientFactory factory) => clientFactory = factory;

        public async Task<HttpResponseMessage> GetAsync(
            Uri url)
        {
            using var client = clientFactory.CreateClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await client.GetAsync(url);
        }
    }
}
