using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class InventoryLogViewModel : ObservableObject
    {
        private readonly IInventoryLogService _invLogService;
        private readonly ISessionService _sessionService;

        [ObservableProperty]
        public partial ObservableCollection<InventoryLogDTO> Logs { get; set; } = [];

        public InventoryLogViewModel(IInventoryLogService invLogService, ISessionService sessionService)
        {
            _invLogService = invLogService;
            _sessionService = sessionService;
        }

        [RelayCommand]
        private async Task Init()
        {
            Logs = new ObservableCollection<InventoryLogDTO>(await _invLogService.GetAllByUserAsync(_sessionService.CurrentUser!.Id));
        }
    }
}
