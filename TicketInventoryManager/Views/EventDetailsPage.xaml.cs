using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class EventDetailsPage : ContentPage
{
	public EventDetailsPage(EventDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}