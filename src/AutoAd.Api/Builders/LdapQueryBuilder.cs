using System;
using System.Text;

namespace AutoAd.Api.Builders
{
    public class LdapQueryBuilder
    {
        private readonly SearchType _searchType;
        private readonly StringBuilder _sb;

        public LdapQueryBuilder(SearchType searchType = SearchType.None, BaseQueryMode baseQueryMode = BaseQueryMode.And)
        {
            _searchType = searchType;
            _sb = new StringBuilder();

            switch (searchType)
            {
                case SearchType.None:
                    _sb.Append("(");
                    break;
                case SearchType.User:
                    _sb.Append("(&(|(objectClass=inetOrgPerson)(objectClass=user))(");
                    break;
                case SearchType.Group:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }

            switch (baseQueryMode)
            {
                case BaseQueryMode.And:
                    _sb.Append("&");
                    break;
                case BaseQueryMode.Or:
                    _sb.Append("|");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(baseQueryMode), baseQueryMode, null);
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
            switch (_searchType)
            {
                case SearchType.None:
                    _sb.Append(")");
                    break;
                case SearchType.User:
                    _sb.Append(")");
                    break;
                case SearchType.Group:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_searchType), _searchType, null);
            }
            _sb.Append(")");
            return _sb.ToString();
        }
    }

    public enum BaseQueryMode : byte
    {
        And = 1,
        Or = 2
    }
}