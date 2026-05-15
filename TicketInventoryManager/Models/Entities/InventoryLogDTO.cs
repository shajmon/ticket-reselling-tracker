using DAL.Enums;

namespace TicketInventoryManager.Models.Entities
{
    public class InventoryLogDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime? SellDate { get; set; }
        public string VenueName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Sector { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal BuyPerOne { get; set; }
        public decimal? SellPerOne { get; set; }
        public string BuyPlatform { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public string SellPlatform { get; set; } = string.Empty;
        public ItemStatus Status { get; set; }

        public decimal? Profit => SellPerOne.HasValue ? (SellPerOne - BuyPerOne) * Quantity : null;
        public bool IsLoss => Profit.HasValue && Profit.Value < 0;
        public decimal? Roi => SellPerOne.HasValue && BuyPerOne > 0 ?
            ((SellPerOne - BuyPerOne) / BuyPerOne) * 100 : null;
    }
}
