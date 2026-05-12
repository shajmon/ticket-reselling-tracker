using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel dashboardVM)
	{
		InitializeComponent();
		BindingContext = dashboardVM;
	}
}