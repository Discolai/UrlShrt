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
        [StringLength(10, MinimumLength = 2)]
        public string Slug { get; set; }

        [Required]
        [StringLength(512)]
        [Url]
        public string RedirectUrl { get; set; }

        public long Clicks { get; set; } = 0;

    }
}
