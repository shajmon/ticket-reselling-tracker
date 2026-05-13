namespace TicketInventoryManager.Views;

public partial class EventsPage : ContentPage
{
	public EventsPage(EventsPage vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}