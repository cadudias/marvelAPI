using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marvel.Api
{
    public interface IHttpClient
    {
        public Task<HttpResponseMessage> GetAsync(Uri url);
    }
}