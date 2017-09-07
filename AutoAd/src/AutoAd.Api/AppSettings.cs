using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoAd.Api
{
    public class AppSettings
    {
        public CorsSettings Cors { get; set; }
    }

    public class CorsSettings
    {
        public bool Enabled { get; set; }

        public string Methods { get; set; }

        public string Origins { get; set; }

        public string Headers { get; set; }
    }
}
