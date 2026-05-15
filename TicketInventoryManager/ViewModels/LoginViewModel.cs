using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;
using TicketInventoryManager.Constants;

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
        private async Task TryLoginAsync()
        {
            ErrorMessage = null;

            UserDTO? loggedUser = await _userService.LoginAsync(Username, Password);

            if (loggedUser == null)
            {
                ErrorMessage = "Invalid username or password";
                return;
            }
            _sessionService.CurrentUser = loggedUser;
            await SuccessfulLoginAsync();
        }

        [RelayCommand]
        private async Task RegisterNewUserAsync()
        {
            ErrorMessage = null;

            if (Username.Length < AppConstants.MinUsernameLength)
            {
                ErrorMessage = $"Username must be at least {AppConstants.MinUsernameLength} characters long";
                return;
            }

            if (Password.Length < AppConstants.MinPasswordLength)
            {
                ErrorMessage = $"Password must be at least {AppConstants.MinPasswordLength} characters long";
                return;
            }

            if (Password != RepeatPassword)
            {
                ErrorMessage = "Passwords do not match";
                return;
            }

            if (await _userService.GetByUsernameAsync(Username) != null)
            {
                ErrorMessage = "Username is already taken";
                return;
            }

            _sessionService.CurrentUser = await _userService.RegisterAsync(Username, Password);
            await SuccessfulLoginAsync();
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

        private async Task SuccessfulLoginAsync()
        {
            await Shell.Current.GoToAsync(AppConstants.DashboardRoute);
        }
    }
}
