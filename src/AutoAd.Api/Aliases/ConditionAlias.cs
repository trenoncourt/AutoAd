namespace AutoAd.Api.Aliases
{
    public class ConditionAlias
    {
        public const string Equal = "=";
        public const string Contains = "contains=";
        public const string StartsWith = "startswith=";
        public const string EndsWith = "endswith=";

        public static readonly string[] ReservedKewords = new [] { "attrs", "base" };
    }
}