using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Dtos
{
    public class UrlItemCreateDto
    {
        [StringLength(10, MinimumLength = 2)]
        [RegularExpression(@"^[a-z,A-Z,0-9]+$", ErrorMessage = "Slugs can only contain the following characters [a-z, A-Z, 0-9].")]
        public string Slug { get; set; }
        
        [Required]
        [Url]
        public string RedirectUrl { get; set; }
    }
}
