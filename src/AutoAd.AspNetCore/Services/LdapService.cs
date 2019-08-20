using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoAd.AspNetCore.Builders;
using AutoAd.AspNetCore.Models;
using AutoAd.AspNetCore.Options;
using AutoAd.AspNetCore.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using LdapEntry = AutoAd.AspNetCore.Models.LdapEntry;

namespace AutoAd.AspNetCore.Services
{
    public class LdapService : ILdapService
    {
        private readonly ILogger<LdapService> _logger;
        private readonly AutoAdOptions _autoAdSettings;

        public LdapService(ILogger<LdapService> logger, IOptions<AutoAdOptions> options)
        {
            _logger = logger;
            _autoAdSettings = options.Value;
        }

        public IEnumerable<LdapEntry> GetEntries(string @base = null, string[] attrs = null, ICollection<Filter> filters = null)
        {
            using (_logger.BeginScope("Get ldap entries"))
            {
                using (var cn = new LdapConnection())
                {
                    if (_autoAdSettings.ReferralFollowing)
                    {
                        _logger.LogDebug("Use referral following");
                        var constraints = cn.SearchConstraints;
                        constraints.ReferralFollowing = true;
                        cn.Constraints = constraints;
                    }

                    _logger.LogDebug("Connect to ldap with host {host} and port {port}", _autoAdSettings.Ldap.Host, _autoAdSettings.Ldap.Port);
                    cn.Connect(_autoAdSettings.Ldap.Host, _autoAdSettings.Ldap.Port);
                    
                    _logger.LogDebug("Bind ldap credentials {user}:{password}", _autoAdSettings.Ldap.User, _autoAdSettings.LogCredentials ? _autoAdSettings.Ldap.Password : "***********");
                    cn.Bind(_autoAdSettings.Ldap.User, _autoAdSettings.Ldap.Password);
                    
                    string currentBase = @base ?? _autoAdSettings.Ldap.Base;
                    if (string.IsNullOrEmpty(currentBase))
                    {
                        _logger.LogError("No base defined in ldap settings");
                        throw new NoNullAllowedException("Ldap base cannot be null or empty.");
                    }
                    _logger.LogDebug("Using {base} base", currentBase);
                    
                    var builder = new LdapQueryBuilder(SearchType.User, filters: filters);
                    
                    string ldapQuery = builder.Build();
                    
                    _logger.LogDebug("generated ldap query: {query}", ldapQuery);
                    
                    LdapSearchResults ldapResults = cn.Search(currentBase, LdapConnection.SCOPE_SUB, ldapQuery, attrs, false);
                    
                    var entries = new List<LdapEntry>();

                    while (ldapResults.hasMore())
                    {
                        Novell.Directory.Ldap.LdapEntry user = ldapResults.next();
                        var entry = new LdapEntry(user.getAttributeSet());
                        entries.Add(entry);
                    }

                    return entries;
                }
            }
        }
        
    }
}