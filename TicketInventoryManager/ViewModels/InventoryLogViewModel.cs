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
        private List<InventoryLogDTO> _allLogs = [];

        [ObservableProperty]
        public partial ObservableCollection<InventoryLogDTO> Logs { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<string> EventNames { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<string> VenueNames { get; set; } = [];

        public HashSet<ItemStatus> StatusFilter { get; set; } = [];

        private string? _selectedEventFilter;
        public string? SelectedEventFilter
        {
            get => _selectedEventFilter;
            set
            {
                if (SetProperty(ref _selectedEventFilter, value))
                    ApplyFilters();
            }
        }

        private string? _selectedVenueFilter;
        public string? SelectedVenueFilter
        {
            get => _selectedVenueFilter;
            set
            {
                if (SetProperty(ref _selectedVenueFilter, value))
                    ApplyFilters();
            }
        }

        public bool IsNotListedSelected
        {
            get => StatusFilter.Contains(ItemStatus.NotListed);
            set
            {
                if (value) StatusFilter.Add(ItemStatus.NotListed);
                else StatusFilter.Remove(ItemStatus.NotListed);
                OnPropertyChanged();
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
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
                ApplyFilters();
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
                    _ = ShowDetailsAsync(value.Id);
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
        private async Task InitAsync()
        {
            await LoadLogsAsync();
        }

        [RelayCommand]
        private void ClearEventFilter()
        {
            SelectedEventFilter = null;
        }

        [RelayCommand]
        private void ClearVenueFilter()
        {
            SelectedVenueFilter = null;
        }

        [RelayCommand]
        private async Task ShowDetailsAsync(int id)
        {
            await Shell.Current.GoToAsync($"logdetail?id={id}");
        }

        [RelayCommand]
        private async Task NewLogAsync()
        {
            await Shell.Current.GoToAsync("logdetail");
        }

        [RelayCommand]
        private async Task GoToDashboardAsync()
        {
            await Shell.Current.GoToAsync("//dashboard");
        }

        [RelayCommand]
        private async Task ExportLogsAsync()
        {
            await _fileService.ExportLogsAsync(Logs);
        }

        [RelayCommand]
        private async Task ImportLogsAsync()
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
            _allLogs = (await _invLogService.GetAllByUserAsync(_sessionService.CurrentUser!.Id)).ToList();
            EventNames = new ObservableCollection<string>(_allLogs.Select(l => l.EventName).Distinct().Order());
            VenueNames = new ObservableCollection<string>(_allLogs.Select(l => l.VenueName).Distinct().Order());
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            var filtered = _allLogs.AsEnumerable();

            if (StatusFilter.Count > 0)
                filtered = filtered.Where(l => StatusFilter.Contains(l.Status));

            if (SelectedEventFilter != null)
                filtered = filtered.Where(l => l.EventName == SelectedEventFilter);

            if (SelectedVenueFilter != null)
                filtered = filtered.Where(l => l.VenueName == SelectedVenueFilter);

            Logs = new ObservableCollection<InventoryLogDTO>(filtered);
        }
    }
}
