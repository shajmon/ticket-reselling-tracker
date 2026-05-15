using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface IUserService
    {
        Task<UserDTO?> GetByUsernameAsync(string username);
        Task<UserDTO?> LoginAsync(string username, string password);
        Task<UserDTO> RegisterAsync(string username, string password);
    }
}
