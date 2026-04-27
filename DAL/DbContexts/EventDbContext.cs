using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DbContexts
{
    public class EventDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=events.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Name = "Taylor Swift",
                    VenueName = "Wembley Stadium",
                    City = "London",
                    Country = "UK",
                    Date = new DateTime(2026, 6, 13),
                    EventType = Enums.EventType.Concert
                },
                new Event
                {
                    Id = 2,
                    Name = "Coldplay",
                    VenueName = "Stade de France",
                    City = "Paris",
                    Country = "France",
                    Date = new DateTime(2026, 7, 4),
                    EventType = Enums.EventType.Concert
                },
                new Event
                {
                    Id = 3,
                    Name = "UFC 317",
                    VenueName = "T-Mobile Arena",
                    City = "Las Vegas",
                    Country = "USA",
                    Date = new DateTime(2026, 6, 27),
                    EventType = Enums.EventType.Sports
                },
                new Event
                {
                    Id = 4,
                    Name = "Hamilton",
                    VenueName = "Victoria Palace Theatre",
                    City = "London",
                    Country = "UK",
                    Date = new DateTime(2026, 8, 1),
                    EventType = Enums.EventType.Theatre
                },
                new Event
                {
                    Id = 5,
                    Name = "Kendrick Lamar",
                    VenueName = "Ziggo Dome",
                    City = "Amsterdam",
                    Country = "Netherlands",
                    Date = new DateTime(2026, 7, 19),
                    EventType = Enums.EventType.Concert
                }
            );
        }
    }
}
