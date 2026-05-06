using DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class InventoryLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime BuyDate { get; set; }
        public DateTime? SellDate { get; set; }
        public string Sector { get; set; }
        public int Quantity { get; set; }
        public decimal BuyPerOne { get; set; }
        public decimal? SellPerOne { get; set; }
        public string BuyPlatform { get; set; }
        public string AccountEmail { get; set; }
        public string SellPlatform { get; set; }
        public ItemStatus Status { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
    }
}
