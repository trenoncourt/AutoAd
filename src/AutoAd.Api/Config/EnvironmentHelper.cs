using System;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace AutoAd.Api.Config
{
    public static class EnvironmentHelper
    {
        public static string GetAspNetCoreEnvironment(string fallbackEnvironment = null)
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                   fallbackEnvironment ?? EnvironmentName.Development;
        }
        
        public static string GetAspNetCoreUrls(string fallbackEnvironment = null)
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ??
                   "http://localhost:5000";
        }
    }
}