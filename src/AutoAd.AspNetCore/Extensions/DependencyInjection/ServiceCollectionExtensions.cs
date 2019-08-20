using System;
using AutoAd.AspNetCore.Options;
using AutoAd.AspNetCore.Services;
using AutoAd.AspNetCore.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AutoAd.AspNetCore.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoAd(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddRouting();
            services.ConfigureAndValidateSingleton<AutoAdOptions>(configuration);
            services.AddScoped<ILdapService, LdapService>();
            return services;
        }
        
        public static IServiceCollection AddAutoAd<TFakeLdapService>(this IServiceCollection services, IConfiguration configuration) 
            where TFakeLdapService : class, ILdapService
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            services.AddRouting();
            services.ConfigureAndValidateSingleton<AutoAdOptions>(configuration);
            services.AddScoped<ILdapService, TFakeLdapService>();
            return services;
        }
        
        public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services
                .AddOptions<TOptions>()
                .Bind(configuration)
                .ValidateDataAnnotations();
            return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }
    }
}