namespace AutoAd.Api.Models
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
        NotEquals = 2,
        Contains = 3,
        StartsWith = 4,
        EndsWith = 5,
        Exist = 6,
        Missing = 7,
        GreaterEqual = 8,
        LessEqual = 9
    }
}