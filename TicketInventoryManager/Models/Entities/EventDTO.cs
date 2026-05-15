using DAL.Enums;

namespace TicketInventoryManager.Models.Entities
{
    public class EventDTO
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public EventType EventType { get; init; }
        public string DisplayName => $"{Name} — {Date:dd MMM yyyy} — {Country}";
    }
}
