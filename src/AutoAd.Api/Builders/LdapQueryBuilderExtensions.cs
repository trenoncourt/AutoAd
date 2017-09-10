using System;
using AutoAd.Api.Models;

namespace AutoAd.Api.Builders
{
    public static class LdapQueryBuilderExtensions
    {
        public static LdapQueryBuilder AddFilter(this LdapQueryBuilder builder, Filter filter)
        {
            switch (filter.Type)
            {
                case FilterType.Equals:
                    builder.Equals(filter.Key, filter.Value);
                    break;
                case FilterType.Contains:
                    builder.Contains(filter.Key, filter.Value);
                    break;
                case FilterType.StartsWith:
                    builder.StartsWith(filter.Key, filter.Value);
                    break;
                case FilterType.EndsWith:
                    builder.EndsWith(filter.Key, filter.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return builder;
        }
    }
}