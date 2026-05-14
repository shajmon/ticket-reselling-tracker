using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventDTO>> GetAllAsync();
        Task<EventDTO?> GetByIdAsync(int id);
        Task AddAsync(EventDTO eventToAdd);
        Task UpdateAsync(EventDTO eventToUpdate);
        Task DeleteAsync(int id);
        Task ImportAsync(IEnumerable<EventDTO> events);
    }
}
