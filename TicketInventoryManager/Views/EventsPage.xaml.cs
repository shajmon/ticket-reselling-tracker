using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class EventsPage : ContentPage
{
	public EventsPage(EventsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ((EventsViewModel)BindingContext).InitCommand.ExecuteAsync(null);
	}
}