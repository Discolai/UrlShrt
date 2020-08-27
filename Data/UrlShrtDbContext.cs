using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShrt.Models;

namespace UrlShrt.Data
{
    public class UrlShrtDbContext : DbContext
    {
        public DbSet<UrlItem> UrlItems { get; set; }
        public UrlShrtDbContext(DbContextOptions<UrlShrtDbContext> options) : base(options) { }
    }
}
