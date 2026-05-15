

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; init; }
        public string Username { get; init; } = string.Empty;
        public string PasswordHash { get; init; } = string.Empty;
        public bool IsAdmin { get; init; }

        public List<InventoryLog> InventoryLogs { get; init; } = [];
    }
}
