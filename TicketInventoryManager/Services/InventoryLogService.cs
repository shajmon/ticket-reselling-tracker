using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public class InventoryLogService : IInventoryLogService
    {
        private readonly AppDbContext _context;
        public InventoryLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InventoryLogDTO log)
        {
            _context.InventoryLogs.Add(FromDTO(log));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var toDelete = await _context.InventoryLogs.FindAsync(id);
            if (toDelete == null)
            {
                return;
            }
            _context.InventoryLogs.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<InventoryLogDTO>> GetAllByUserAsync(int userId)
        {
            return await _context.InventoryLogs
                .Where(log => log.UserId == userId)
                .Include(log => log.Event)
                .Select(log => ToDTO(log))
                .ToListAsync();
        }

        public async Task<InventoryLogDTO?> GetByIdAsync(int id)
        {
            var target = await _context.InventoryLogs
                            .Include(log => log.Event)
                            .FirstOrDefaultAsync(log => log.Id == id);
            return target == null ? null : ToDTO(target);
        }

        public async Task UpdateAsync(InventoryLogDTO newLog)
        {
            var oldLog = await _context.InventoryLogs.FindAsync(newLog.Id);
            if (oldLog == null)
            {
                throw new KeyNotFoundException("ID does not exist in the database");
            }
            UpdateEntity(oldLog, newLog);
            await _context.SaveChangesAsync();
        }

        private static InventoryLogDTO ToDTO(InventoryLog logToMap)
        {
            return new InventoryLogDTO
            {
                Id = logToMap.Id,
                UserId = logToMap.UserId,
                EventId = logToMap.EventId,
                EventName = logToMap.Event.Name,
                EventDate = logToMap.Event.Date,
                BuyDate = logToMap.BuyDate,
                SellDate = logToMap.SellDate,
                VenueName = logToMap.Event.VenueName,
                Country = logToMap.Event.Country,
                City = logToMap.Event.City,
                Sector = logToMap.Sector,
                Quantity = logToMap.Quantity,
                BuyPerOne = logToMap.BuyPerOne,
                SellPerOne = logToMap.SellPerOne,
                BuyPlatform = logToMap.BuyPlatform,
                AccountEmail = logToMap.AccountEmail,
                SellPlatform = logToMap.SellPlatform,
                Status = logToMap.Status
            };
        }

        private static InventoryLog FromDTO(InventoryLogDTO logToMap)
        {
            return new InventoryLog
            {
                UserId = logToMap.UserId,
                EventId = logToMap.EventId,
                BuyDate = logToMap.BuyDate,
                SellDate = logToMap.SellDate,
                Sector = logToMap.Sector,
                Quantity = logToMap.Quantity,
                BuyPerOne = logToMap.BuyPerOne,
                SellPerOne = logToMap.SellPerOne,
                BuyPlatform = logToMap.BuyPlatform,
                AccountEmail = logToMap.AccountEmail,
                SellPlatform = logToMap.SellPlatform,
                Status = logToMap.Status
            };
        }

        private static void UpdateEntity(InventoryLog oldLog, InventoryLogDTO newLog)
        {
            oldLog.EventId = newLog.EventId;
            oldLog.BuyDate = newLog.BuyDate;
            oldLog.SellDate = newLog.SellDate;
            oldLog.Sector = newLog.Sector;
            oldLog.Quantity = newLog.Quantity;
            oldLog.BuyPerOne = newLog.BuyPerOne;
            oldLog.SellPerOne = newLog.SellPerOne;
            oldLog.BuyPlatform = newLog.BuyPlatform;
            oldLog.AccountEmail = newLog.AccountEmail;
            oldLog.SellPlatform = newLog.SellPlatform;
            oldLog.Status = newLog.Status;
        }
    }
}
