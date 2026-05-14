using System.Collections.ObjectModel;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface IFileService
    {
        Task<bool> ExportLogsAsync(ObservableCollection<InventoryLogDTO> logs);
        Task<ObservableCollection<InventoryLogDTO>?> ImportLogsAsync();
        Task<bool> ExportEventsAsync(ObservableCollection<EventDTO> events);
        Task<ObservableCollection<EventDTO>?> ImportEventsAsync();
    }
}
