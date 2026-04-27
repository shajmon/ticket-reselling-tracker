using System;
using System.Collections.Generic;
using System.Text;

namespace TicketInventoryManager.Models.Entities
{
    public class InventoryLogDTO
    {
        public int Id { get; set; } 
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime? SellDate { get; set; }
        public string VenueName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Sector { get; set; }
        public int Quantity { get; set; }
        public decimal BuyPerOne { get; set; }
        public decimal? SellPerOne { get; set; }
        public string BuyPlatform { get; set; }
        public string AccountEmail { get; set; }
        public string SellPlatform { get; set; }
        public Enums.TicketStatus Status { get; set; }

        public decimal? Profit => SellPerOne.HasValue ? (SellPerOne - BuyPerOne) * Quantity : null;
        public decimal? Roi => SellPerOne.HasValue && BuyPerOne > 0 ?
            ((SellPerOne - BuyPerOne) / BuyPerOne) * 100 : null;
        public int? DaysHeld => SellDate.HasValue ? SellDate.Value.Subtract(BuyDate).Days : null;
    }
}
