using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Helpers
{
    public class UrlHelper
    {
        public static string Combine(string baseUrl, params string[] relUrls)
        {
            return $"{baseUrl.TrimEnd('/')}/{string.Join("/", relUrls.Select(u => u.TrimStart('/').TrimEnd('/')))}";
        }
    }
}
