using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        [ObservableProperty]
        public partial string Username { get; set; }
        [ObservableProperty]
        public partial string Password { get; set; }
        [ObservableProperty]
        public partial string RepeatPassword { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoggingIn))]
        public partial bool IsRegistering { get; set; }
        public bool IsLoggingIn => !IsRegistering;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        public partial string? ErrorMessage { get; set; }
        public bool HasError => ErrorMessage != null;

        public LoginViewModel(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
            Username = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
            IsRegistering = false;
        }

        [RelayCommand]
        private async Task TryLogin()
        {
            ErrorMessage = null;

            UserDTO? loggedUser = await _userService.LoginAsync(Username, Password);

            if (loggedUser == null)
            {
                ErrorMessage = "Invalid username or password";
                return;
            }
            _sessionService.CurrentUser = loggedUser;
            await SuccessfulLogin();
        }

        [RelayCommand]
        private async Task RegisterNewUser()
        {
            ErrorMessage = null;

            if (Username.Length < 3)
            {
                ErrorMessage = "Username must be at least 3 characters long";
                return;
            }

            if (Password.Length < 8)
            {
                ErrorMessage = "Password must be at least 8 characters long";
                return;
            }

            if (Password == RepeatPassword)
            {
                _sessionService.CurrentUser = await _userService.RegisterAsync(Username, Password);
                await SuccessfulLogin();
            }
            else
            {
                ErrorMessage = "Passwords do not match";
            }
        }

        [RelayCommand]
        private void ChangeMode()
        {
            ErrorMessage = null;
            RepeatPassword = string.Empty;

            if (IsRegistering)
            {
                Username = string.Empty;
                Password = string.Empty;
            }

            IsRegistering = !IsRegistering;
        }

        private async Task SuccessfulLogin()
        {
            await Shell.Current.GoToAsync("//dashboard");
        }
    }
}
