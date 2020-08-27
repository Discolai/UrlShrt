using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Models
{
    public class UrlItem
    {
        [Key]
        [Range(2, 10, ErrorMessage = "Please specify a {0} between the range {1} and {2}")]
        public string Slug { get; set; }

        [Required]
        [MaxLength(512, ErrorMessage = "Please specify a {0} less than {1} characters")]
        [Url]
        public string RedirectUrl { get; set; }

        public long Clicks { get; set; } = 0;

    }
}
