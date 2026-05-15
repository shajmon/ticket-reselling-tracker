using TicketInventoryManager.Constants;
using TicketInventoryManager.Views;

namespace TicketInventoryManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(AppConstants.LogDetailRoute, typeof(InventoryLogDetailsPage));
            Routing.RegisterRoute(AppConstants.EventDetailRoute, typeof(EventDetailsPage));
        }
    }
}
