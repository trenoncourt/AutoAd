namespace AutoAd.Api.Models
{
    public class Filter
    {
        public FilterType Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
    
    public enum FilterType : byte
    {
        Equals = 1,
        NotEquals = 2,
        Contains = 3,
        StartsWith = 4,
        EndsWith = 5,
        Exist = 6,
        Missing = 7,
        GreaterEqual = 8,
        LessEqual = 9,
        OnlyActiveUsers = 10
    }
}