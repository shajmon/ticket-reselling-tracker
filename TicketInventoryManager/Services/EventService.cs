using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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
            var eventEntity = FromDTO(eventToAdd);
            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var toDelete = await _context.Events.FindAsync(id);
            if (toDelete == null)
            {
                return;
            }
            _context.Events.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventDTO>> GetAllAsync()
        {
            return await _context.Events
                .Select(currentEvent => ToDTO(currentEvent))
                .ToListAsync();
        }

        public async Task<EventDTO?> GetByIdAsync(int id)
        {
            var target = await _context.Events.FindAsync(id);
            return target == null ? null : ToDTO(target);
        }

        public async Task UpdateAsync(EventDTO newLog)
        {
            var oldLog = await _context.Events.FindAsync(newLog.Id);
            if (oldLog == null)
            {
                throw new KeyNotFoundException("ID does not exist in the database");
            }
            UpdateEntity(newLog, oldLog);
            await _context.SaveChangesAsync();
        }

        private EventDTO ToDTO(Event eventToMap)
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

        private Event FromDTO(EventDTO eventToMap)
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

        private void UpdateEntity(EventDTO newLog, Event oldLog)
        {
            oldLog.Name = newLog.Name;
            oldLog.VenueName = newLog.VenueName;
            oldLog.City = newLog.City;
            oldLog.Country = newLog.Country;
            oldLog.Date = newLog.Date;
        }
    }
}
