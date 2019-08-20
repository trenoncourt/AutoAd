using Novell.Directory.Ldap;

namespace AutoAd.AspNetCore.Models
{
    public class LdapEntry
    {
        public LdapEntry()
        {
            
        }

        public LdapEntry(LdapAttributeSet ldapAttributeSet)
        {
            foreach (LdapAttribute ldapAttribute in ldapAttributeSet)
            {
                switch (ldapAttribute.Name)
                {
                    case "cn":
                        Cn = ldapAttribute.StringValue;
                        break;
                    case "displayName":
                        DisplayName = ldapAttribute.StringValue;
                        break;
                    case "userPrincipalName":
                        UserPrincipalName = ldapAttribute.StringValue;
                        break;
                    case "title":
                        Title = ldapAttribute.StringValue;
                        break;
                    case "givenName":
                        GivenName = ldapAttribute.StringValue;
                        break;
                    case "sn":
                        Sn = ldapAttribute.StringValue;
                        break;
                    case "initials":
                        Initials = ldapAttribute.StringValue;
                        break;
                    case "description":
                        Description = ldapAttribute.StringValue;
                        break;
                }
            }
        }
        
        /// <summary>
        /// Common Name <example>John Smith</example>
        /// </summary>
        public string Cn { get; set; }

        /// <summary>
        /// Display Name <example>John Smith</example>
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// User Logon Name <example>JSmith@domain.com</example>
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Title <example>Manager</example>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// First Name <example>John</example>
        /// </summary>
        public string GivenName { get; set; }
        
        /// <summary>
        /// Last Name <example>Smith</example>
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// Initials <example>JS</example>
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// Description <example>Sales Manager</example>
        /// </summary>
        public string Description { get; set; }
    }
}