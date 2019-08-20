using AutoAd.AspNetCore.Options;

namespace AutoAd.Api
{
    public class AppSettings
    {
        public CorsSettings Cors { get; set; }

        public LdapOptions Ldap { get; set; }
    }

    public class CorsSettings
    {
        public bool Enabled { get; set; }

        public string Methods { get; set; }

        public string Origins { get; set; }

        public string Headers { get; set; }
    }

	
}
