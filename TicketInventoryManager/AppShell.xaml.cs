using TicketInventoryManager.Views;

namespace TicketInventoryManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("logdetail", typeof(InventoryLogDetailsPage));
        }
    }
}
