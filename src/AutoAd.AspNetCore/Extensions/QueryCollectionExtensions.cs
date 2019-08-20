using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoAd.AspNetCore.Aliases;
using AutoAd.AspNetCore.Models;
using Microsoft.AspNetCore.Http;

namespace AutoAd.AspNetCore.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static ICollection<Filter> GetFilters(this IQueryCollection queryCollection)
        {
            ICollection<Filter> filters = new List<Filter>();
            foreach (var queryPart in queryCollection)
            {
                if (FilterAlias.ReservedKewords.Any(kw => kw.EndsWith(queryPart.Key, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                
                if (queryPart.Key.EndsWith(FilterAlias.EndsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.EndsWith, Key = queryPart.Key.ReplaceIgnoreCase(FilterAlias.EndsWith, ""), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(FilterAlias.StartsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.StartsWith, Key = queryPart.Key.ReplaceIgnoreCase(FilterAlias.StartsWith, ""), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(FilterAlias.Contains, StringComparison.InvariantCultureIgnoreCase))
                {
                    filters.Add(new Filter { Type = FilterType.Contains, Key = queryPart.Key.ReplaceIgnoreCase(FilterAlias.Contains, ""), Value = queryPart.Value });
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

        public static string ReplaceIgnoreCase(this string input, string oldValue, string newValue)
        {
            return input.ToLower().Replace(oldValue, newValue);
        }
    }
}