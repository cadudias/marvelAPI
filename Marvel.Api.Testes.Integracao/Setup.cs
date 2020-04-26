using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marvel.Api.Testes.Integracao
{
    [SetUpFixture]
    public class Setup
    {
        private const string urlApi = "http://localhost:5000/";
        public static HttpClient Client { get; private set; }
        private IServiceScope serviceScope;
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetupAsync()
        {
            var factory = new WebApplicationFactory<Startup>();

            factory.WithWebHostBuilder(builder =>
                builder.UseEnvironment("Testing")
                .ConfigureServices(services =>
                {
                    //services.RemoveAll(typeof(IHttpClient));

                    //services.AddServiceWithFaker<IHttpClient>(service => A.CallTo(() =>
                    //service.GetAsync(new Uri("http://localhost:5001")))
                    //.Returns(new HttpResponseMessage(HttpStatusCode.BadRequest)));
                }
            ));


            Client = factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri("http://localhost:5000") });
            serviceScope = factory.Services.CreateScope();
            ServiceProvider = serviceScope.ServiceProvider;
        }
    }
 }