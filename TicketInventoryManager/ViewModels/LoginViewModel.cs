using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserService _userService;
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
        public partial bool IsLoading { get; set; }
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasLoginError))]
        public partial string? LoginError { get; set; }
        public bool HasLoginError => LoginError != null;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasRegistrationError))]
        public partial string? RegistrationError { get; set; }
        public bool HasRegistrationError => RegistrationError != null;

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            Username = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
            IsRegistering = false;
            IsLoading = false;
        }

        [RelayCommand]
        private async Task TryLogin()
        {
            IsLoading = true;
            LoginError = null;

            UserDTO? loggedUser = await _userService.LoginAsync(Username, Password);

            IsLoading = false;

            if (loggedUser == null)
            {
                LoginError = "Invalid username or password";
                return;
            }
            SuccesfulLogin();
        }

        [RelayCommand]
        private async Task RegisterNewUser()
        {
            IsLoading = true;
            RegistrationError = null;

            if (Password == RepeatPassword)
            {
                await _userService.RegisterAsync(Username, Password);
                IsLoading = false;
                SuccesfulLogin();
            }
            RegistrationError = "Passwords do not match";
            IsLoading = false;
        }

        [RelayCommand]
        private void ChangeMode()
        {
            if (IsRegistering)
            {
                Username = string.Empty;
                Password = string.Empty;
            }

            IsRegistering = !IsRegistering;
        }

        private void SuccesfulLogin()
        {
            throw new NotImplementedException();
        }
    }
}
