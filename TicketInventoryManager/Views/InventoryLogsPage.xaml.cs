using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class InventoryLogsPage : ContentPage
{
	public InventoryLogsPage(InventoryLogViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}