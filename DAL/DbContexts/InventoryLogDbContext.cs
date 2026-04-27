using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DbContexts
{
    public class InventoryLogDbContext : DbContext
    {
        public DbSet<User> InventoryLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=inventorylogs.db");
    }
}
