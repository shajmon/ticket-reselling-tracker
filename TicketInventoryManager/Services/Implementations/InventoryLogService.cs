using DAL;
using DAL.Entities;
using DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketInventoryManager.Models.DataSummary;
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
            await Task.Run(() =>
            {
                _context.InventoryLogs.Add(FromDTO(log));
                _context.SaveChanges();
            });
        }

        public async Task DeleteAsync(int id)
        {
            await Task.Run(() =>
            {
                var toDelete = _context.InventoryLogs.Find(id);
                if (toDelete == null) return;
                _context.InventoryLogs.Remove(toDelete);
                _context.SaveChanges();
            });
        }

        public async Task<IEnumerable<InventoryLogDTO>> GetAllByUserAsync(int userId)
        {
            return await Task.Run(() => _context.InventoryLogs
                .AsNoTracking()
                .Where(log => log.UserId == userId)
                .Include(log => log.Event)
                .Select(log => ToDTO(log))
                .ToList());
        }

        public async Task<InventoryLogDTO?> GetByIdAsync(int id)
        {
            return await Task.Run(() =>
            {
                var target = _context.InventoryLogs
                    .AsNoTracking()
                    .Include(log => log.Event)
                    .FirstOrDefault(log => log.Id == id);
                return target == null ? null : ToDTO(target);
            });
        }

        public async Task UpdateAsync(InventoryLogDTO newLog)
        {
            await Task.Run(() =>
            {
                var oldLog = _context.InventoryLogs.Find(newLog.Id)
                    ?? throw new KeyNotFoundException("ID does not exist in the database");
                UpdateEntity(oldLog, newLog);
                _context.SaveChanges();
            });
        }

        //SQLite cannot reliably parse decimal calculation results - that's why I'm using so many casts
        public async Task<DashboardSummary> GetSummaryAsync(int userId, DateTime from, DateTime to, int? eventId = null)
        {
            return await Task.Run(() =>
            {
                var baseQuery = _context.InventoryLogs
                    .AsNoTracking()
                    .Where(l => l.UserId == userId &&
                               ( eventId == null || 
                               l.EventId == eventId));

                var buysQuery = baseQuery.Where(l => l.BuyDate >= from && l.BuyDate <= to);
                var salesQuery = baseQuery
                    .Where(l => l.SellPerOne != null &&
                                  l.SellDate >= from && 
                                  l.SellDate <= to);

                var buysRaw = buysQuery
                    .GroupBy(_ => 1)
                    .Select(g => new
                    {
                        TicketsBought = g.Sum(l => l.Quantity),
                        UnsoldTickets = g.Sum(l => (l.Status == ItemStatus.NotListed || 
                                                    l.Status == ItemStatus.Listed) ? l.Quantity : 0),
                        TotalSpent = g.Sum(l => (double) l.BuyPerOne * l.Quantity),
                        TotalUnsoldRetailValue = g.Sum(l => (l.Status == ItemStatus.NotListed || 
                                                             l.Status == ItemStatus.Listed) ? 
                                                            (double) l.BuyPerOne * l.Quantity : 
                                                            0.0),
                    })
                    .FirstOrDefault();

                decimal totalSpent = (decimal) (buysRaw?.TotalSpent ?? 0);
                int ticketsBought = buysRaw?.TicketsBought ?? 0;

                var buysSummary = new BuysSummary(
                    ticketsBought,
                    buysRaw?.UnsoldTickets ?? 0,
                    totalSpent,
                    ticketsBought > 0 ? totalSpent / ticketsBought : 0,
                    (decimal) (buysRaw?.TotalUnsoldRetailValue ?? 0));

                var salesRaw = salesQuery
                    .GroupBy(_ => 1)
                    .Select(g => new
                    {
                        TicketsSold = g.Sum(l => l.Quantity),
                        TotalRevenue = g.Sum(l => (double) l.SellPerOne!.Value * l.Quantity),
                        TotalProfit = g.Sum(l => ((double) l.SellPerOne!.Value - (double) l.BuyPerOne) * l.Quantity),
                    })
                    .FirstOrDefault();

                var bestEventRaw = salesRaw == null ? null : salesQuery
                    .GroupBy(l => l.EventId)
                    .Select(g => new
                    {
                        EventId = g.Key,
                        Profit = g.Sum(l => ((double) l.SellPerOne!.Value - (double) l.BuyPerOne) * l.Quantity),
                        Spend = g.Sum(l => (double) l.BuyPerOne * l.Quantity),
                    })
                    .OrderByDescending(x => x.Profit)
                    .FirstOrDefault();

                EventDTO? bestEvent = null;
                if (bestEventRaw != null)
                {
                    bestEvent = _context.Events
                        .AsNoTracking()
                        .Where(e => e.Id == bestEventRaw.EventId)
                        .Select(e => new EventDTO
                        {
                            Id = e.Id,
                            Name = e.Name,
                            VenueName = e.VenueName,
                            City = e.City,
                            Country = e.Country,
                            Date = e.Date,
                            EventType = e.EventType
                        })
                        .FirstOrDefault();
                }

                var salesSummary = new SalesSummary(
                    salesRaw?.TicketsSold ?? 0,
                    (decimal) (salesRaw?.TotalRevenue ?? 0),
                    (decimal) (salesRaw?.TotalProfit ?? 0),
                    (decimal) (bestEventRaw?.Profit ?? 0),
                    (decimal) (bestEventRaw?.Spend ?? 0),
                    bestEvent);

                return new DashboardSummary(buysSummary, salesSummary);
            });
        }

        public async Task<int> ImportAsync(IEnumerable<InventoryLogDTO> logs, int userId, bool replace)
        {
            return await Task.Run(() =>
            {
                if (replace)
                {
                    var existing = _context.InventoryLogs.Where(l => l.UserId == userId).ToList();
                    _context.InventoryLogs.RemoveRange(existing);
                }

                int imported = 0;
                foreach (var log in logs)
                {
                    var matchedEvent = _context.Events
                        .FirstOrDefault(e => e.Name == log.EventName && e.Date == log.EventDate);
                    if (matchedEvent == null) continue;

                    _context.InventoryLogs.Add(new InventoryLog
                    {
                        UserId = userId,
                        EventId = matchedEvent.Id,
                        BuyDate = log.BuyDate,
                        SellDate = log.SellDate,
                        Sector = log.Sector,
                        Quantity = log.Quantity,
                        BuyPerOne = log.BuyPerOne,
                        SellPerOne = log.SellPerOne,
                        BuyPlatform = log.BuyPlatform,
                        AccountEmail = log.AccountEmail,
                        SellPlatform = log.SellPlatform,
                        Status = log.Status
                    });
                    imported++;
                }

                _context.SaveChanges();
                return imported;
            });
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
