using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutoAd.Api.Config
{
    public static class ConfigurationHelper
    {
        public static IConfiguration CreateDefaultConfiguration(string[] args = null)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvironmentHelper.GetAspNetCoreEnvironment()}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (args != null)
            {
                builder.AddCommandLine(args);
            }
            return builder.Build();
        }
    }
}