using System;
using System.Collections.Generic;
using System.Linq;
using AutoAd.Api.Aliases;
using AutoAd.Api.Aliases.Models;
using Microsoft.AspNetCore.Http;

namespace AutoAd.Api.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static IEnumerable<Condition> GetConditions(this IQueryCollection queryCollection)
        {
            ICollection<Condition> conditions = new List<Condition>();
            foreach (var queryPart in queryCollection)
            {
                if (ConditionAlias.ReservedKewords.Any(kw => kw.EndsWith(queryPart.Key, StringComparison.InvariantCultureIgnoreCase)))
                    continue;
                
                if (queryPart.Key.EndsWith(ConditionAlias.EndsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(new Condition { Type = ConditionType.EndsWith, Key = queryPart.Key.Replace(ConditionAlias.EndsWith, "", StringComparison.InvariantCultureIgnoreCase), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(ConditionAlias.StartsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(new Condition { Type = ConditionType.StartsWith, Key = queryPart.Key.Replace(ConditionAlias.StartsWith, "", StringComparison.InvariantCultureIgnoreCase), Value = queryPart.Value });
                }
                else if (queryPart.Key.EndsWith(ConditionAlias.Contains, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(new Condition { Type = ConditionType.Contains, Key = queryPart.Key.Replace(ConditionAlias.Contains, "", StringComparison.InvariantCultureIgnoreCase), Value = queryPart.Value });
                }
                else
                {
                    conditions.Add(new Condition { Type = ConditionType.Equals, Key = queryPart.Key, Value = queryPart.Value });
                }
            }
            return conditions;
        }
    }
}