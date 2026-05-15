namespace TicketInventoryManager.Models.Entities
{
    public class UserDTO
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public bool IsAdmin { get; init; }
    }
}
