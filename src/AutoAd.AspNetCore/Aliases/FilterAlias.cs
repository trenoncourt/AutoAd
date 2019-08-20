﻿namespace AutoAd.AspNetCore.Aliases
{
    public class FilterAlias
    {
        public const string Contains = "contains";
        public const string StartsWith = "startswith";
        public const string EndsWith = "endswith";
        public const string NotEquals = "!";
        public const string Exist = "exist";
        public const string GreaterEquals  = ">";
        public const string LessEquals  = "<";
        public const string OnlyActiveUsers  = "only_active_users";

        public static readonly string[] ReservedKewords = { "attrs", "base" };
    }
}