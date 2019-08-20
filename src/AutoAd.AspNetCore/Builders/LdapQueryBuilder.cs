using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoAd.AspNetCore.Models;

namespace AutoAd.AspNetCore.Builders
{
    public class LdapQueryBuilder
    {
        private readonly SearchType _searchType;
        private readonly BaseQueryMode _baseQueryMode;
        private readonly ICollection<Filter> _filters;
        private readonly StringBuilder _sb;

        public bool HasFilters => _filters?.Any() == true;

        public LdapQueryBuilder(SearchType searchType = SearchType.None, BaseQueryMode baseQueryMode = BaseQueryMode.And, ICollection<Filter> filters = null)
        {
            _searchType = searchType;
            _baseQueryMode = baseQueryMode;
            _filters = filters;
            _sb = new StringBuilder();
            
            switch (_searchType)
            {
                case SearchType.None:
                    _sb.Append("(");
                    break;
                case SearchType.User:
                    _sb.Append($"{(HasFilters ? "(&" : "" )}(|(objectClass=inetOrgPerson)(objectClass=user)){(HasFilters ? "(" : "" )}");
                    break;
                case SearchType.Group:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_searchType), _searchType, null);
            }
            
            AddFilters();
        }
        
        public void AddFilters()
        {
            if (HasFilters)
            {
                switch (_baseQueryMode)
                {
                    case BaseQueryMode.And:
                        _sb.Append("&");
                        break;
                    case BaseQueryMode.Or:
                        _sb.Append("|");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_baseQueryMode), _baseQueryMode, null);
                }
                
                foreach (Filter filter in _filters)
                {
                    AddFilter(filter);
                }
            }
        }

        public void AddFilter(Filter filter)
        {
            switch (filter.Type)
            {
                case FilterType.Equals:
                    Equals(filter.Key, filter.Value);
                    break;
                case FilterType.Contains:
                    Contains(filter.Key, filter.Value);
                    break;
                case FilterType.StartsWith:
                    StartsWith(filter.Key, filter.Value);
                    break;
                case FilterType.EndsWith:
                    EndsWith(filter.Key, filter.Value);
                    break;
                case FilterType.OnlyActiveUsers:
                    OnlyActiveUsers(filter.Key, filter.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public LdapQueryBuilder Equals(string key, string value)
        {
            _sb.AppendFormat("({0}={1})", key, value);
            return this;
        }
        
        public LdapQueryBuilder NotEquals(string key, string value)
        {
            _sb.AppendFormat("(!({0}={1}))", key, value);
            return this;
        }
        
        public LdapQueryBuilder Contains(string key, string value)
        {
            _sb.AppendFormat("({0}=*{1}*)", key, value);
            return this;
        }
        
        public LdapQueryBuilder StartsWith(string key, string value)
        {
            _sb.AppendFormat("({0}={1}*)", key, value);
            return this;
        }
        
        public LdapQueryBuilder EndsWith(string key, string value)
        {
            _sb.AppendFormat("({0}=*{1})", key, value);
            return this;
        }

        public LdapQueryBuilder OnlyActiveUsers(string key, string value)
        {
            if (value == "true" || value == "1")
            {
                _sb.Append("(!(userAccountControl:1.2.840.113556.1.4.803:=2))");
            }
            return this;
        }

        public LdapQueryBuilder Exist(string key, string value)
        {
            if (value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                _sb.AppendFormat("({0}=*)", key);   
            }
            else if (value.Equals("false", StringComparison.InvariantCultureIgnoreCase))
            {
                _sb.AppendFormat("(!({0}=*))", key);
            }
            return this;
        }
        
        public LdapQueryBuilder GreaterEquals(string key, string value)
        {
            _sb.AppendFormat("({0}>={1})", key, value);
            return this;
        }
        
        public LdapQueryBuilder LessEquals(string key, string value)
        {
            _sb.AppendFormat("({0}<={1})", key, value);
            return this;
        }

        public string Build()
        {
            _sb.Append($"{(HasFilters ? "))" : "" )}");
            return _sb.ToString();
        }
    }

    public enum BaseQueryMode : byte
    {
        And = 1,
        Or = 2
    }
}