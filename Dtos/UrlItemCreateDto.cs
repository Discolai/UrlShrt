using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Dtos
{
    public class UrlItemCreateDto
    {
        public string Slug { get; set; }
        public string RedirectUrl { get; set; }
    }
}
