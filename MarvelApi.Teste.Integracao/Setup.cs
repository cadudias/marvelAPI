using FakeItEasy;
using MarvelApi.Testes.Compartilhado;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarvelApi
{
    [SetUpFixture]
    public class Setup
    {
        private const string urlApi = "http://localhost:5000/";
        private WebApplicationFactory<Startup> factory;
        public static HttpClient Client { get; private set; }
        private IServiceScope serviceScope;
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        //[OneTimeSetUp]
        //public async Task OneTimeSetupAsync()
        //{
        //    CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

        //    var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Test.Integration.json");

        //    //var webHost = new WebHostBuilder()
        //    //     .ConfigureTestServices(services =>
        //    //     {
        //    //         services.AddServiceWithFaker<IHttpClient>(x => A.CallTo(() => x.GetAsync(A<Uri>._))
        //    //                .Returns(new HttpResponseMessage(HttpStatusCode.BadRequest)));
        //    //     })
        //    //     .UseStartup<Startup>();

        //    factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        //        builder.UseEnvironment("Development")
        //        .ConfigureServices(services =>
        //        {
        //            //services.AddServiceWithFaker<IHttpClient>(x => A.CallTo(() => x.GetAsync(A<Uri>._))
        //            //.Returns(new HttpResponseMessage(HttpStatusCode.BadRequest)));

        //            //services.AddServiceWithFaker<Relogio>(x => A.CallTo(() => x.ObterStringTicks())
        //            //.Returns("123456778900"));
        //            // Build the service provider.
        //            var sp = services.BuildServiceProvider();

        //            // Create a scope to obtain a reference to the database
        //            // context (ApplicationDbContext).
        //            using (var scope = sp.CreateScope())
        //            {
        //                var scopedServices = scope.ServiceProvider;
        //                //ServiceProvider = scopedServices.ServiceProvider;
        //                Configuration = ServiceProvider.GetService<IConfiguration>();

        //            }
        //        }).ConfigureAppConfiguration((context, configuration) =>
        //        {
        //            configuration.AddJsonFile(configPath);
        //        }));

        //    //var testserver = new TestServer(webHost);
        //    //Client = testserver.CreateClient();

        //    // aqui o .net chama o Startup e usa os servicos dele no teste
        //    //Client = factory.CreateClient(new WebApplicationFactoryClientOptions
        //    //{
        //    //    BaseAddress = new Uri(urlApi)
        //    //});

        //   // var response = await Client.GetAsync("personagem");

        //    //serviceScope = factory.Services.CreateScope();
        //   // ServiceProvider = serviceScope.ServiceProvider;
        //    //Configuration = ServiceProvider.GetService<IConfiguration>();
        //}
        public void Dispose()
        {
            Client.Dispose();
            serviceScope.Dispose();
            factory.Dispose();
        }
    }
}