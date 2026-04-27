using System;
using System.Collections.Generic;
using System.Text;

namespace TicketInventoryManager.Models.Entities
{
    public class InventoryLogDTO
    {
        public int Id { get; set; } 
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public string VenueName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Sector { get; set; }
        public int Quantity { get; set; }
        public decimal BuyPerOne { get; set; }
        public decimal? SellPerOne { get; set; }

    }
}
