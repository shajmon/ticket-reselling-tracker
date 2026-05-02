using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface IInventoryLogService
    {
        Task<IEnumerable<InventoryLogDTO>> GetAllByUserAsync(int userId);
        Task<InventoryLogDTO?> GetByIdAsync(int id);
        Task AddAsync(InventoryLogDTO log);
        Task UpdateAsync(InventoryLogDTO log);
        Task DeleteAsync(int id);
    }
}
