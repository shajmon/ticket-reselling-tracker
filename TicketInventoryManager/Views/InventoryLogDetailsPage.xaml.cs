using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class InventoryLogDetailsPage : ContentPage
{
	public InventoryLogDetailsPage(InventoryLogDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
        base.OnNavigatedTo(args);
        await ((InventoryLogDetailsViewModel)BindingContext).InitCommand.ExecuteAsync(null);
    }
}