using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class InventoryLogsPage : ContentPage
{
	public InventoryLogsPage(InventoryLogViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override async void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		await ((InventoryLogViewModel)BindingContext).InitCommand.ExecuteAsync(null);
	}
}