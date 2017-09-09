namespace AutoAd.Api
{
    public class AppSettings
    {
        public CorsSettings Cors { get; set; }

        public LdapSettings Ldap { get; set; }
    }

    public class CorsSettings
    {
        public bool Enabled { get; set; }

        public string Methods { get; set; }

        public string Origins { get; set; }

        public string Headers { get; set; }
    }

	public class LdapSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Base { get; set; }
    }
}
