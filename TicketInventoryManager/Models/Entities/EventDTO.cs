using DAL.Enums;

namespace TicketInventoryManager.Models.Entities
{
    public class EventDTO
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string VenueName { get ; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Date { get; set; }
        public EventType EventType { get; init; }
    }
}
