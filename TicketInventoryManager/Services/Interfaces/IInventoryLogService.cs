using DAL.Enums;
using TicketInventoryManager.Models.DataSummary;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface IInventoryLogService
    {
        Task<IEnumerable<InventoryLogDTO>> GetAllByUserAsync(int userId);
        Task<IEnumerable<InventoryLogDTO>> GetAllByUserAsync(int userId, HashSet<ItemStatus> statusFilter);
        Task<InventoryLogDTO?> GetByIdAsync(int id);
        Task AddAsync(InventoryLogDTO log);
        Task UpdateAsync(InventoryLogDTO log);
        Task DeleteAsync(int id);
        Task<DashboardSummary> GetSummaryAsync(int userId, DateTime from, DateTime to, int? eventId = null);
        Task<int> ImportAsync(IEnumerable<InventoryLogDTO> logs, int userId, bool replace);
    }
}
