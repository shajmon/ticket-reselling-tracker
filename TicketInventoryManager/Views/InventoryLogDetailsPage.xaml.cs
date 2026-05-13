using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class InventoryLogDetailsPage : ContentPage
{
	public InventoryLogDetailsPage(InventoryLogDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}