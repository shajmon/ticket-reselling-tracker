using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO?> GetByUsernameAsync(string username);
        Task<UserDTO?> LoginAsync(string username, string password);
        Task<UserDTO> RegisterAsync(string username, string password);
        Task DeleteAsync(int id);
    }
}
