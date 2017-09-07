using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Novell.Directory.Ldap;
using System.Collections.Generic;
using System.IO;
using System.Reflection.PortableExecutable;

namespace AutoAd.Api
{
    public class Program
    {
        private static AppSettings AppSettings;

        public static void Main()
        {
            BuildWebHost().Run();
        }

        public static IWebHost BuildWebHost() =>
            new WebHostBuilder()
                .UseKestrel(options => options.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    AppSettings = config.Build().Get<AppSettings>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    }
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    if (AppSettings.Cors.Enabled)
                    {
                        services.AddCors();
                    }
                })
                .Configure(app =>
                {
                    app.UseRouter(r =>
                    {
                        r.MapGet("users", async context =>
                        {              
                            using (var cn = new LdapConnection())
                            {
                                cn.Connect(AppSettings.Ldap.Host, AppSettings.Ldap.Port);
                                cn.Bind(AppSettings.Ldap.User, AppSettings.Ldap.Password);

                                string @base = context.Request.Query["base"].ToString();
                                if (string.IsNullOrEmpty(@base))
                                {
                                    @base = AppSettings.Ldap.Base;
                                }
                                if (string.IsNullOrEmpty(@base))
                                {
                                    context.Response.StatusCode = 400;
                                    await context.Response.WriteAsync("No base defined.");
                                }

                                var t = cn.Search("CN=Users,DC=Mobilis,DC=local", LdapConnection.SCOPE_SUB, "(cn=*)", new[] { "userPrincipalName", "displayName", "primaryGroupID", "title" }, false);
                                var bite = new List<LdapEntry>();
                                while (t.hasMore())
                                {
                                    bite.Add(t.next());
                                }
                            }
                            await context.Response.WriteAsync("");
                        });
                    });
                })
                .Build();
    }
}

