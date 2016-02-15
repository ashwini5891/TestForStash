using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace TheWorld.Models
{
    public class QuoteContext : DbContext
    {
        public DbSet<Quote> Quotes { get; set; }
    }
}
