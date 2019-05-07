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
using AutoAd.Api.Builders;
using AutoAd.Api.Extensions;
using AutoAd.Api.Models;
using Newtonsoft.Json.Linq;

namespace AutoAd.Api
{
    public class Program
    {
        private static AppSettings _appSettings;

        public static void Main()
        {
            BuildWebHost().Run();
        }

        public static IWebHost BuildWebHost() =>
            new WebHostBuilder()
                .UseKestrel(options => options.AddServerHeader = false)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    _appSettings = config.Build().Get<AppSettings>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                    }
                })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    if (_appSettings.Cors.Enabled)
                    {
                        services.AddCors();
                    }
                })
                .Configure(app =>
                {
                    app.ConfigureCors(_appSettings);
                    app.UseRouter(r =>
                    {
                        r.MapGet("users", async context =>
                        {
                            using (var cn = new LdapConnection())
                            {
                                if (_appSettings.ReferralFollowing)
                                {
                                    // todo(tre): check if "cn.Constraints.ReferralFollowing = true" is the same
                                    
                                    var constraints = cn.SearchConstraints;
                                    constraints.ReferralFollowing = true;
                                    cn.Constraints = constraints;
                                }

                                cn.Connect(_appSettings.Ldap.Host, _appSettings.Ldap.Port);
                                cn.Bind(_appSettings.Ldap.User, _appSettings.Ldap.Password);

                                string @base = GetBase(context);
                                if (string.IsNullOrEmpty(@base))
                                {
                                    context.Response.StatusCode = 400;
                                    await context.Response.WriteAsync("No base defined.");
                                    return;
                                }

                                string[] attrs = GetAttrs(context);
                                
                                IEnumerable<Filter> filters = context.Request.Query.GetFilters().ToList();
                                var builder = new LdapQueryBuilder(SearchType.User);
                                if (filters.Any())
                                {
                                    foreach (Filter filter in filters)
                                    {
                                        builder.AddFilter(filter);
                                    }
                                }

                                string ldapQuery = builder.Build();

                                LdapSearchResults ldapResults = cn.Search(@base, LdapConnection.SCOPE_SUB, ldapQuery, attrs, false);
                                var entries = new List<LdapEntry>();

                                var array = new JArray();
                                while (ldapResults.hasMore())
                                {
                                    LdapEntry user = ldapResults.next();
                                    var attrSet = user.getAttributeSet();
                                    
                                    JObject @object = new JObject();
                                    foreach (LdapAttribute ldapAttribute in attrSet)
                                    {
                                        @object.Add(ldapAttribute.Name, ldapAttribute.StringValue);
                                    }

                                    array.Add(@object);
                                    entries.Add(user);
                                }
                                
                                string json = array.ToString();
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(json);
                            }
                        });
                    });
                })
                .Build();

        private static string GetBase(HttpContext context)
        {
            string @base = context.Request.Query["base"].ToString();
            if (string.IsNullOrEmpty(@base))
            {
                @base = _appSettings.Ldap.Base;
            }

            return @base;
        }
        
        private static string[] GetAttrs(HttpContext context)
        {
            if (!context.Request.Query.ContainsKey("attrs"))
                return null;
            return context.Request.Query["attrs"].ToString().Split(',');
        }
    }
}

