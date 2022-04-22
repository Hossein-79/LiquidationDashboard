using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiquidationDashboard.Models;

namespace LiquidationDashboard.Data
{
    public class LiquidationContext : DbContext
    {
        public LiquidationContext(DbContextOptions<LiquidationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Symbol> Symbols { get; set; }
        public DbSet<Storage> Storages { get; set; }
    }
}
