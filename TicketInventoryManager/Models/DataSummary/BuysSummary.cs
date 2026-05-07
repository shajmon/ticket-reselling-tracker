namespace TicketInventoryManager.Models.DataSummary
{
    public record BuysSummary(
        int TicketsBought,
        int UnsoldTickets,
        decimal TotalSpent,
        decimal AverageTicketBuyPrice,
        decimal TotalUnsoldRetailValue);
}
