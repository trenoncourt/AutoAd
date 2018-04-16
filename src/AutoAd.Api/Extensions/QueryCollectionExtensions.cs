using System;
using System.Collections.Generic;
using System.Linq;
using AutoAd.Api.Aliases;
using AutoAd.Api.Models;
using Microsoft.AspNetCore.Http;

namespace AutoAd.Api.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static IEnumerable<Filter> GetFilters(this IQueryCollection queryCollection)
        {
            ICollection<Filter> filters = new List<Filter>();
            foreach (var queryPart in queryCollection)
            {
                if (FilterAlias.ReservedKewords.Any(kw => kw.EndsWith(queryPart.Key, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                
                if (queryPart.Key.EndsWith(FilterAlias.EndsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.EndsWith, Key = queryPart.Key.Replace(FilterAlias.EndsWith, "", StringComparison.InvariantCultureIgnoreCase), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(FilterAlias.StartsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.StartsWith, Key = queryPart.Key.Replace(FilterAlias.StartsWith, "", StringComparison.InvariantCultureIgnoreCase), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(FilterAlias.Contains, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.Contains, Key = queryPart.Key.Replace(FilterAlias.Contains, "", StringComparison.InvariantCultureIgnoreCase), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(FilterAlias.OnlyActiveUsers, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.OnlyActiveUsers, Key = queryPart.Key, Value = queryPart.Value });
                }
                else
                {
                    filters.Add(new Filter { Type = FilterType.Equals, Key = queryPart.Key, Value = queryPart.Value });
                }
            }
            return filters;
        }
    }
}