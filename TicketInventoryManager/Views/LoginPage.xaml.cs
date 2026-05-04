using TicketInventoryManager.ViewModels;

namespace TicketInventoryManager.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginVM)
	{
        InitializeComponent();
		BindingContext = loginVM;
	}
}