using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Marvel.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Relogio>();
            services.AddTransient<PersonagemService>();
            services.AddTransient<IHttpClient, CustomHttpClient>();

            services.AddSingleton(x =>
            {
                var configuracao = x.GetService<IConfiguration>();
                var uri = new Uri(configuracao.GetValue<string>("MarvelComicsAPI:BaseURL"));
                var publicKey = configuracao.GetValue<string>("MarvelComicsAPI:PublicKey");
                var privateKey = configuracao.GetValue<string>("MarvelComicsAPI:PrivateKey");
                return new Configuracao(uri, publicKey, privateKey);
            });


            services.AddControllers();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
