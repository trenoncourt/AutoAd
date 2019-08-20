using System.Collections.Generic;
using AutoAd.AspNetCore.Models;

namespace AutoAd.AspNetCore.Services.Abstractions
{
    public interface ILdapService
    {
        IEnumerable<LdapEntry> GetEntries(string @base = null, string[] attrs = null, ICollection<Filter> filters = null);
    }
}