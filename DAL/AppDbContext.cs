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
            modelBuilder.Entity<InventoryLog>().HasData(
                // Taylor Swift (EventId=1)
                new InventoryLog
                {
                    Id = 1,
                    UserId = 1,
                    EventId = 1,
                    Sector = "Floor",
                    Quantity = 2,
                    BuyPerOne = 180m,
                    SellPerOne = 320m,
                    BuyDate = new DateTime(2026, 1, 10),
                    SellDate = new DateTime(2026, 2, 5),
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 2,
                    UserId = 1,
                    EventId = 1,
                    Sector = "Floor",
                    Quantity = 4,
                    BuyPerOne = 195m,
                    SellPerOne = 350m,
                    BuyDate = new DateTime(2026, 1, 15),
                    SellDate = new DateTime(2026, 3, 1),
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 3,
                    UserId = 1,
                    EventId = 1,
                    Sector = "Standing",
                    Quantity = 2,
                    BuyPerOne = 120m,
                    SellPerOne = 210m,
                    BuyDate = new DateTime(2026, 2, 3),
                    SellDate = new DateTime(2026, 4, 10),
                    BuyPlatform = "SeatGeek",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 4,
                    UserId = 1,
                    EventId = 1,
                    Sector = "Block A",
                    Quantity = 3,
                    BuyPerOne = 150m,
                    SellPerOne = null,
                    BuyDate = new DateTime(2026, 3, 20),
                    SellDate = null,
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Listed
                },
                new InventoryLog
                {
                    Id = 5,
                    UserId = 1,
                    EventId = 1,
                    Sector = "Block B",
                    Quantity = 1,
                    BuyPerOne = 160m,
                    SellPerOne = null,
                    BuyDate = new DateTime(2026, 4, 2),
                    SellDate = null,
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.NotListed
                },
                // Coldplay (EventId=2)
                new InventoryLog
                {
                    Id = 6,
                    UserId = 1,
                    EventId = 2,
                    Sector = "Pit",
                    Quantity = 2,
                    BuyPerOne = 95m,
                    SellPerOne = 160m,
                    BuyDate = new DateTime(2026, 1, 22),
                    SellDate = new DateTime(2026, 3, 14),
                    BuyPlatform = "Fnac",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 7,
                    UserId = 1,
                    EventId = 2,
                    Sector = "Pit",
                    Quantity = 4,
                    BuyPerOne = 90m,
                    SellPerOne = 145m,
                    BuyDate = new DateTime(2026, 2, 11),
                    SellDate = new DateTime(2026, 3, 28),
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 8,
                    UserId = 1,
                    EventId = 2,
                    Sector = "Tribune",
                    Quantity = 2,
                    BuyPerOne = 65m,
                    SellPerOne = 100m,
                    BuyDate = new DateTime(2026, 2, 20),
                    SellDate = new DateTime(2026, 4, 5),
                    BuyPlatform = "Fnac",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 9,
                    UserId = 1,
                    EventId = 2,
                    Sector = "Tribune",
                    Quantity = 3,
                    BuyPerOne = 70m,
                    SellPerOne = null,
                    BuyDate = new DateTime(2026, 3, 8),
                    SellDate = null,
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Listed
                },
                new InventoryLog
                {
                    Id = 10,
                    UserId = 1,
                    EventId = 2,
                    Sector = "VIP",
                    Quantity = 1,
                    BuyPerOne = 250m,
                    SellPerOne = 420m,
                    BuyDate = new DateTime(2026, 3, 15),
                    SellDate = new DateTime(2026, 5, 2),
                    BuyPlatform = "SeatGeek",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.ToDeliver
                },
                // UFC 317 (EventId=3)
                new InventoryLog
                {
                    Id = 11,
                    UserId = 1,
                    EventId = 3,
                    Sector = "Octagon Side",
                    Quantity = 2,
                    BuyPerOne = 300m,
                    SellPerOne = 550m,
                    BuyDate = new DateTime(2026, 2, 1),
                    SellDate = new DateTime(2026, 3, 20),
                    BuyPlatform = "UFC.com",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 12,
                    UserId = 1,
                    EventId = 3,
                    Sector = "Upper Bowl",
                    Quantity = 4,
                    BuyPerOne = 120m,
                    SellPerOne = 195m,
                    BuyDate = new DateTime(2026, 2, 14),
                    SellDate = new DateTime(2026, 4, 1),
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 13,
                    UserId = 1,
                    EventId = 3,
                    Sector = "Floor",
                    Quantity = 2,
                    BuyPerOne = 450m,
                    SellPerOne = null,
                    BuyDate = new DateTime(2026, 3, 5),
                    SellDate = null,
                    BuyPlatform = "UFC.com",
                    SellPlatform = "",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Listed
                },
                new InventoryLog
                {
                    Id = 14,
                    UserId = 1,
                    EventId = 3,
                    Sector = "Upper Bowl",
                    Quantity = 3,
                    BuyPerOne = 110m,
                    SellPerOne = 170m,
                    BuyDate = new DateTime(2026, 4, 10),
                    SellDate = new DateTime(2026, 5, 8),
                    BuyPlatform = "SeatGeek",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.ToDeliver
                },
                // Hamilton (EventId=4)
                new InventoryLog
                {
                    Id = 15,
                    UserId = 1,
                    EventId = 4,
                    Sector = "Stalls",
                    Quantity = 2,
                    BuyPerOne = 85m,
                    SellPerOne = 140m,
                    BuyDate = new DateTime(2026, 1, 30),
                    SellDate = new DateTime(2026, 3, 10),
                    BuyPlatform = "ATG",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 16,
                    UserId = 1,
                    EventId = 4,
                    Sector = "Royal Circle",
                    Quantity = 2,
                    BuyPerOne = 110m,
                    SellPerOne = 185m,
                    BuyDate = new DateTime(2026, 2, 8),
                    SellDate = new DateTime(2026, 4, 22),
                    BuyPlatform = "ATG",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 17,
                    UserId = 1,
                    EventId = 4,
                    Sector = "Stalls",
                    Quantity = 4,
                    BuyPerOne = 80m,
                    SellPerOne = null,
                    BuyDate = new DateTime(2026, 3, 25),
                    SellDate = null,
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.NotListed
                },
                new InventoryLog
                {
                    Id = 18,
                    UserId = 1,
                    EventId = 4,
                    Sector = "Grand Circle",
                    Quantity = 2,
                    BuyPerOne = 60m,
                    SellPerOne = 95m,
                    BuyDate = new DateTime(2026, 4, 14),
                    SellDate = new DateTime(2026, 5, 1),
                    BuyPlatform = "ATG",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                // Kendrick Lamar (EventId=5)
                new InventoryLog
                {
                    Id = 19,
                    UserId = 1,
                    EventId = 5,
                    Sector = "Floor",
                    Quantity = 2,
                    BuyPerOne = 75m,
                    SellPerOne = 130m,
                    BuyDate = new DateTime(2026, 2, 25),
                    SellDate = new DateTime(2026, 4, 3),
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 20,
                    UserId = 1,
                    EventId = 5,
                    Sector = "Floor",
                    Quantity = 4,
                    BuyPerOne = 80m,
                    SellPerOne = 135m,
                    BuyDate = new DateTime(2026, 3, 3),
                    SellDate = new DateTime(2026, 4, 18),
                    BuyPlatform = "SeatGeek",
                    SellPlatform = "StubHub",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Delivered
                },
                new InventoryLog
                {
                    Id = 21,
                    UserId = 1,
                    EventId = 5,
                    Sector = "Seated",
                    Quantity = 3,
                    BuyPerOne = 55m,
                    SellPerOne = null,
                    BuyDate = new DateTime(2026, 3, 18),
                    SellDate = null,
                    BuyPlatform = "Ticketmaster",
                    SellPlatform = "",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.Listed
                },
                new InventoryLog
                {
                    Id = 22,
                    UserId = 1,
                    EventId = 5,
                    Sector = "VIP",
                    Quantity = 1,
                    BuyPerOne = 200m,
                    SellPerOne = 340m,
                    BuyDate = new DateTime(2026, 4, 7),
                    SellDate = new DateTime(2026, 5, 5),
                    BuyPlatform = "SeatGeek",
                    SellPlatform = "Viagogo",
                    AccountEmail = "admin@test.com",
                    Status = ItemStatus.ToDeliver
                }
            );
        }
    }
}
