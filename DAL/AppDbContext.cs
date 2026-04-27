using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=app.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    IsAdmin = true
                }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Name = "Taylor Swift",
                    VenueName = "Wembley Stadium",
                    City = "London",
                    Country = "UK",
                    Date = new DateTime(2026, 6, 13),
                    EventType = EventType.Concert
                },
                new Event
                {
                    Id = 2,
                    Name = "Coldplay",
                    VenueName = "Stade de France",
                    City = "Paris",
                    Country = "France",
                    Date = new DateTime(2026, 7, 4),
                    EventType = EventType.Concert
                },
                new Event
                {
                    Id = 3,
                    Name = "UFC 317",
                    VenueName = "T-Mobile Arena",
                    City = "Las Vegas",
                    Country = "USA",
                    Date = new DateTime(2026, 6, 27),
                    EventType = EventType.Sports
                },
                new Event
                {
                    Id = 4,
                    Name = "Hamilton",
                    VenueName = "Victoria Palace Theatre",
                    City = "London",
                    Country = "UK",
                    Date = new DateTime(2026, 8, 1),
                    EventType = EventType.Theatre
                },
                new Event
                {
                    Id = 5,
                    Name = "Kendrick Lamar",
                    VenueName = "Ziggo Dome",
                    City = "Amsterdam",
                    Country = "Netherlands",
                    Date = new DateTime(2026, 7, 19),
                    EventType = EventType.Concert
                }
            );
        }
    }
}
