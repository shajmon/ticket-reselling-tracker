using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using System.Collections.ObjectModel;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class InventoryLogViewModel : ObservableObject
    {
        private readonly IInventoryLogService _invLogService;
        private readonly ISessionService _sessionService;
        private readonly IFileService _fileService;

        [ObservableProperty]
        public partial ObservableCollection<InventoryLogDTO> Logs { get; set; } = [];

        public HashSet<ItemStatus> StatusFilter { get; set; } = [];

        public bool IsNotListedSelected
        {
            get => StatusFilter.Contains(ItemStatus.NotListed);
            set
            {
                if (value) StatusFilter.Add(ItemStatus.NotListed);
                else StatusFilter.Remove(ItemStatus.NotListed);
                OnPropertyChanged();
                _ = LoadLogsAsync();
            }
        }

        public bool IsListedSelected
        {
            get => StatusFilter.Contains(ItemStatus.Listed);
            set
            {
                if (value) StatusFilter.Add(ItemStatus.Listed);
                else StatusFilter.Remove(ItemStatus.Listed);
                OnPropertyChanged();
                _ = LoadLogsAsync();
            }
        }

        public bool IsToDeliverSelected
        {
            get => StatusFilter.Contains(ItemStatus.ToDeliver);
            set
            {
                if (value) StatusFilter.Add(ItemStatus.ToDeliver);
                else StatusFilter.Remove(ItemStatus.ToDeliver);
                OnPropertyChanged();
                _ = LoadLogsAsync();
            }
        }

        public bool IsDeliveredSelected
        {
            get => StatusFilter.Contains(ItemStatus.Delivered);
            set
            {
                if (value) StatusFilter.Add(ItemStatus.Delivered);
                else StatusFilter.Remove(ItemStatus.Delivered);
                OnPropertyChanged();
                _ = LoadLogsAsync();
            }
        }

        private InventoryLogDTO? _selectedLog;
        public InventoryLogDTO? SelectedLog
        {
            get => _selectedLog;
            set
            {
                if (SetProperty(ref _selectedLog, value) && value != null)
                {
                    _ = ShowDetails(value.Id);
                    SetProperty(ref _selectedLog, null);
                }
            }
        }

        public InventoryLogViewModel(IInventoryLogService invLogService, ISessionService sessionService, IFileService fileService)
        {
            _invLogService = invLogService;
            _sessionService = sessionService;
            _fileService = fileService;
        }

        [RelayCommand]
        private async Task Init()
        {
            await LoadLogsAsync();
        }

        [RelayCommand]
        private async Task ShowDetails(int id)
        {
            await Shell.Current.GoToAsync($"logdetail?id={id}");
        }

        [RelayCommand]
        private async Task NewLog()
        {
            await Shell.Current.GoToAsync("logdetail");
        }

        [RelayCommand]
        private async Task GoToDashboard()
        {
            await Shell.Current.GoToAsync("//dashboard");
        }

        [RelayCommand]
        private async Task ExportLogs()
        {
            await _fileService.ExportLogsAsync(Logs);
        }

        [RelayCommand]
        private async Task ImportLogs()
        {
            var imported = await _fileService.ImportLogsAsync();
            if (imported == null) return;

            bool replace = await Shell.Current.DisplayAlertAsync(
                "Import mode",
                "Do you want to replace your existing logs or append to them?",
                "Replace",
                "Append");

            int count = await _invLogService.ImportAsync(imported, _sessionService.CurrentUser!.Id, replace);
            await LoadLogsAsync();
            await Shell.Current.DisplayAlertAsync("Import complete", $"{count} log(s) imported.", "OK");
        }

        private async Task LoadLogsAsync()
        {
            Logs = new ObservableCollection<InventoryLogDTO>(
                await _invLogService.GetAllByUserAsync(_sessionService.CurrentUser!.Id, StatusFilter));
        }
    }
}
