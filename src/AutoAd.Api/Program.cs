using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoAd.Api.Config;
using AutoAd.Api.Extensions;
using AutoAd.AspNetCore;
using AutoAd.AspNetCore.Builders;
using AutoAd.AspNetCore.Extensions;
using AutoAd.AspNetCore.Extensions.DependencyInjection;
using AutoAd.AspNetCore.Models;
using Newtonsoft.Json.Linq;
using Serilog;
using LdapEntry = Novell.Directory.Ldap.LdapEntry;

namespace AutoAd.Api
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static async Task Main(string[] args)
        {
            try
            {
                _configuration = ConfigurationHelper.CreateDefaultConfiguration(args);
                Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger();
                IWebHostBuilder webHostBuilder = CreateWebHostBuilder(args);
                Log.Information("Web Host created");
                IWebHost webHost = webHostBuilder.Build();
                Log.Information("Web Host builded, starting...");
                var applicationLifetime = webHost.Services.GetService<IApplicationLifetime>();
                applicationLifetime.ApplicationStarted.Register(() => Log.Information("Web Host started"));
                await webHost.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string environment = EnvironmentHelper.GetAspNetCoreEnvironment();
            string urls = EnvironmentHelper.GetAspNetCoreUrls();
            Log.Information("Current environment: {environment}, urls used: {urls}", environment, urls);
            _configuration = _configuration ?? ConfigurationHelper.CreateDefaultConfiguration(args);
            var appOptions = _configuration.Get<AppSettings>();
            Log.Debug("Configuration used: {@configuration}", appOptions);

            return new WebHostBuilder()
                .SuppressStatusMessages(true)
                .UseEnvironment(environment)
//                .UseUrls(urls)
                .UseKestrel()
                .ConfigureKestrel(options => options.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(_configuration)
                .ConfigureServices(services =>
                {
                    if (appOptions.Cors.Enabled)
                    {
                        services.AddCors();
                    }

                    services.AddAutoAd(_configuration);
                })
                .UseSerilog()
                .UseIIS()
                .UseIISIntegration()
                .UseDefaultServiceProvider((context, options) => options.ValidateScopes = context.HostingEnvironment.IsDevelopment())
                .Configure(app =>
                {
                    app.ConfigureCors(appOptions);
                    app.UseAutoAd();
                });
        }
    }
}

