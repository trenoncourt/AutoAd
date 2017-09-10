using System;
using System.Text;

namespace AutoAd.Api.Builders
{
    public class LdapQueryBuilder
    {
        private readonly StringBuilder _sb;

        public LdapQueryBuilder(BaseQueryMode baseQueryMode = BaseQueryMode.And)
        {
            _sb = new StringBuilder();
            _sb.Append("(");

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

        public string Build()
        {
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