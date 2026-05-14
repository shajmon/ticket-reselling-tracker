using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;

        public EventService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EventDTO eventToAdd)
        {
            await Task.Run(() =>
            {
                _context.Events.Add(FromDTO(eventToAdd));
                _context.SaveChanges();
            });
        }

        public async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                var toDelete = _context.Events.Find(id);
                if (toDelete == null) return;
                _context.Events.Remove(toDelete);
                _context.SaveChanges();
            });
        }

        public async Task<IEnumerable<EventDTO>> GetAllAsync()
        {
            return await Task.Run(() => _context.Events
                .AsNoTracking()
                .Select(currentEvent => ToDTO(currentEvent))
                .ToList());
        }

        public async Task<EventDTO?> GetByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                var target = _context.Events.AsNoTracking().FirstOrDefault(e => e.Id == id);
                return target == null ? null : ToDTO(target);
            });
        }

        public async Task UpdateAsync(EventDTO newEvent)
        {
            await Task.Run(() =>
            {
                var oldEvent = _context.Events.Find(newEvent.Id)
                    ?? throw new KeyNotFoundException("ID does not exist in the database");
                UpdateEntity(newEvent, oldEvent);
                _context.SaveChanges();
            });
        }

        public async Task ImportAsync(IEnumerable<EventDTO> events)
        {
            await Task.Run(() =>
            {
                foreach (var e in events)
                {
                    var exists = _context.Events
                        .Any(dbEvent => dbEvent.Name == e.Name && dbEvent.Date == e.Date);
                    if (!exists)
                        _context.Events.Add(FromDTO(e));
                }
                _context.SaveChanges();
            });
        }

        private static EventDTO ToDTO(Event eventToMap)
        {
            return new EventDTO
            {
                Id = eventToMap.Id,
                Name = eventToMap.Name,
                VenueName = eventToMap.VenueName,
                City = eventToMap.City,
                Country = eventToMap.Country,
                Date = eventToMap.Date,
                EventType = eventToMap.EventType
            };
        }

        private static Event FromDTO(EventDTO eventToMap)
        {
            return new Event
            {
                Name = eventToMap.Name,
                VenueName = eventToMap.VenueName,
                City = eventToMap.City,
                Country = eventToMap.Country,
                Date = eventToMap.Date,
                EventType = eventToMap.EventType
            };
        }

        private static void UpdateEntity(EventDTO newEvent, Event oldEvent)
        {
            oldEvent.Name = newEvent.Name;
            oldEvent.VenueName = newEvent.VenueName;
            oldEvent.City = newEvent.City;
            oldEvent.Country = newEvent.Country;
            oldEvent.Date = newEvent.Date;
        }
    }
}
