using System.ComponentModel.DataAnnotations;

namespace AutoAd.AspNetCore.Options
{
    public class AutoAdOptions
    {
        [Required]
        public LdapOptions Ldap { get; set; }
        
        public bool ReferralFollowing { get; set; }

        public bool LogCredentials { get; set; }

        public bool UseFakeData { get; set; }
    }
    
    public class LdapOptions
    {
        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }
        
        public string User { get; set; }

        public string Password { get; set; }

        public string Base { get; set; }
    }
}