using DAL.Enums;
using TicketInventoryManager.Models.DataSummary;
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
        Task<DashboardSummary> GetSummaryAsync(int userId, DateTime from, DateTime to,
                                                      HashSet<ItemStatus> statusFilter,
                                                      int? eventId = null);
    }
}
