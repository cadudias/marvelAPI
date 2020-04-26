using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarvelApi
{
    public interface IHttpClient
    {
        public Task<HttpResponseMessage> GetAsync(Uri url);
    }
}