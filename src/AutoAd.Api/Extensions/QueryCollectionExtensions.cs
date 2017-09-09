using System;
using System.Collections.Generic;
using System.Linq;
using AutoAd.Api.Aliases;
using AutoAd.Api.Aliases.Models;
using System.Text.RegularExpressions;

namespace AutoAd.Api.Extensions
{
    public static class QueryCollectionExtensions
    {
        public static IEnumerable<Condition> GetConditions(this string[] queryParts)
        {
            ICollection<Condition> conditions = new List<Condition>();
            foreach (var queryPart in queryParts)
            {
                if(ConditionAlias.ReservedKewords.Contains(queryPart.Split('=')[0], StringComparer.InvariantCultureIgnoreCase))
                    continue;
                
                if (queryPart.Contains(ConditionAlias.EndsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(GetCondition(ConditionAlias.EndsWith, ConditionType.EndsWith, queryPart));
                }
                else if (queryPart.Contains(ConditionAlias.StartsWith, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(GetCondition(ConditionAlias.StartsWith, ConditionType.StartsWith, queryPart));
                }
                else if (queryPart.Contains(ConditionAlias.Contains, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(GetCondition(ConditionAlias.Contains, ConditionType.Contains, queryPart));
                }
                else if (queryPart.Contains(ConditionAlias.Equal, StringComparison.InvariantCultureIgnoreCase))
                {
                    conditions.Add(GetCondition(ConditionAlias.Equal, ConditionType.Equal, queryPart));
                }
            }
            return conditions;
        }

        private static Condition GetCondition(string alias, ConditionType type, string queryPart)
        {
            string[] parts = Regex.Split(queryPart, alias, RegexOptions.IgnoreCase);
            return new Condition
            {
                Type = type,
                Key = parts[0],
                Value = parts[1]
            };
        }
    }
}