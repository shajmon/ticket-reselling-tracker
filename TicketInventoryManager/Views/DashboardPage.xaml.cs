using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel dashboardVM)
	{
		InitializeComponent();
		BindingContext = dashboardVM;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		await ((DashboardViewModel)BindingContext).InitCommand.ExecuteAsync(null);
	}
}