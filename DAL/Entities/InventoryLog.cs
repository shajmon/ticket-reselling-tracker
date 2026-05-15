using DAL.Enums;


namespace DAL.Entities
{
    public class InventoryLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime? SellDate { get; set; }
        public string Sector { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal BuyPerOne { get; set; }
        public decimal? SellPerOne { get; set; }
        public string BuyPlatform { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public string SellPlatform { get; set; } = string.Empty;
        public ItemStatus Status { get; set; }

        public User User { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}
