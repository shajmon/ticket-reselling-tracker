using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class EventDetailsPage : ContentPage
{
	public EventDetailsPage(EventDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ((EventDetailsViewModel)BindingContext).InitCommand.ExecuteAsync(null);
	}
}