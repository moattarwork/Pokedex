using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pokedex.Core.Clients;
using Pokedex.WebApi.Options;
using Refit;

namespace Pokedex.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOptions<TOption>(this IServiceCollection services,
            IConfiguration configuration) where TOption : class
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.AddOptions<TOption>()
                .Bind(configuration.GetSection(typeof(TOption).Name))
                .ValidateDataAnnotations();

            return services;
        }

        public static IServiceCollection AddClient<TClient>(this IServiceCollection services, Func<Clients, string> urlProvider)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (urlProvider == null) throw new ArgumentNullException(nameof(urlProvider));
            
            services.AddRefitClient<IPokeApiClient>(c =>new RefitSettings(new NewtonsoftJsonContentSerializer()))
                .ConfigureHttpClient((sp,c) =>
                {
                    var clients = sp.GetRequiredService<IOptions<Clients>>().Value;
                    var url = urlProvider.Invoke(clients);
                    
                    c.BaseAddress = new Uri(url);
                });

            return services;
        }
    }
}