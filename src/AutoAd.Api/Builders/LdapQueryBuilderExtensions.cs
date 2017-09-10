using System;
using AutoAd.Api.Models;

namespace AutoAd.Api.Builders
{
    public static class LdapQueryBuilderExtensions
    {
        public static LdapQueryBuilder AddCondition(this LdapQueryBuilder builder, Condition condition)
        {
            switch (condition.Type)
            {
                case ConditionType.Equals:
                    builder.Equals(condition.Key, condition.Value);
                    break;
                case ConditionType.Contains:
                    builder.Contains(condition.Key, condition.Value);
                    break;
                case ConditionType.StartsWith:
                    builder.StartsWith(condition.Key, condition.Value);
                    break;
                case ConditionType.EndsWith:
                    builder.EndsWith(condition.Key, condition.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return builder;
        }
    }
}