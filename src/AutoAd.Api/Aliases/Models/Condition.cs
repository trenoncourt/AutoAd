namespace AutoAd.Api.Aliases.Models
{
    public class Condition
    {
        public ConditionType Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }

    public enum ConditionType : byte
    {
        Equals = 1,
        Contains = 2,
        StartsWith = 3,
        EndsWith = 4
    }
}