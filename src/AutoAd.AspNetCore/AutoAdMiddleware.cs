using System.Collections.Generic;
using AutoAd.AspNetCore.Extensions;
using AutoAd.AspNetCore.Models;
using AutoAd.AspNetCore.Services.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AutoAd.AspNetCore
{
    
    public static class AutoAdMiddlewareExtensions
    {
        public static IApplicationBuilder UseAutoAd(this IApplicationBuilder builder)
        {
            return builder.UseRouter(r => {
                r.MapGet("/users", async context =>
                {
                    ILdapService ldapService = context.RequestServices.GetService<ILdapService>();
                    IEnumerable<LdapEntry> ldapEntries = ldapService.GetEntries(GetBase(context), GetAttrs(context), context.Request.Query.GetFilters());

                    await context.WriteResponseBodyAsync(ldapEntries);
                }); 
            });
        }
        
        private static string GetBase(HttpContext context)
        {
            if (!context.Request.Query.ContainsKey("base"))
                return null;
            
            return context.Request.Query["base"].ToString();
        }
        
        private static string[] GetAttrs(HttpContext context)
        {
            if (!context.Request.Query.ContainsKey("attrs"))
                return null;
            
            return context.Request.Query["attrs"].ToString().Split(',');
        }
    }
}