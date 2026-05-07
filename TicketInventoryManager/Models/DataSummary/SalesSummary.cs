using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Models.DataSummary
{
    public record SalesSummary(
        int TicketsSold,
        decimal TotalRevenue,
        decimal TotalProfit,
        decimal BestProfit,
        decimal BestEventSpend,
        TimeSpan AverageHoldTime,
        EventDTO BestEvent);
}
