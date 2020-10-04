using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Dtos
{
    public class AnalyticsViewDto
    {
        public string ShortUrl { get; set; }
        public int Clicks { get; set; }
    }
}
