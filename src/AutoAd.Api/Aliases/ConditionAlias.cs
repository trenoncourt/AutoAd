namespace AutoAd.Api.Aliases
{
    public class ConditionAlias
    {
        public const string Contains = "contains";
        public const string StartsWith = "startswith";
        public const string EndsWith = "endswith";
        public const string NotEquals = "!";
        public const string Exist = "exist";

        public static readonly string[] ReservedKewords = { "attrs", "base" };
    }
}