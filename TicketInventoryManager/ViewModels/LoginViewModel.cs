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
        public bool Registration { get; set; }

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            Username = string.Empty;
            Password = string.Empty;
            RepeatPassword = string.Empty;
            Registration = false;
        }

        [RelayCommand]
        public async Task TryLogin()
        {
            UserDTO? loggedUser = await _userService.LoginAsync(Username, Password);

            if (loggedUser == null)
            {
                //invalid username or pass stverec
            } 
            else
            {
                //reroute na dashboard 
                //pass userDTO do dalsieho vm
            }
        }

        [RelayCommand]
        public async Task RegisterNewUser()
        {
            if (Password == RepeatPassword)
            {
                await _userService.RegisterAsync(Username, Password);
                //reroute na dashboard
                //pass userDTO do dalsieho vm
            }
            else
            {
                //passwords nematchuju stverec
            }
        }

        [RelayCommand]
        public void ChangeMode()
        {
            Registration = !Registration;
        }
    }
}
