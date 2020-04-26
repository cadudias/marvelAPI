using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarvelApi.Testes.Compartilhado
{
    public static class FakerServiceCollectionExtesions
    {
        public static void AddServiceWithFaker<T>(this IServiceCollection services, Action<T>? configure = null) where T : class
        {
            services.AddTransient(sp =>
            {
                var faker = FakerServiceProvider.Get<T>();
                if (faker == null)
                {
                    faker = A.Fake<T>();
                    configure?.Invoke(faker);
                    FakerServiceProvider.Add(faker);
                }
                return faker;
            });
        }
    }
}
