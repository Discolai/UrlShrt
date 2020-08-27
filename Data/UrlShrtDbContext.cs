using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Data
{
    public class UrlShrtDbContext : DbContext
    {
        public UrlShrtDbContext(DbContextOptions<UrlShrtDbContext> options) : base(options) { }
    }
}
