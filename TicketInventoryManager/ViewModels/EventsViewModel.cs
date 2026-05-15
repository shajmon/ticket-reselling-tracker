using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketInventoryManager.Constants;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class EventsViewModel : ObservableObject
    {
        private readonly IEventService _eventService;
        private readonly IFileService _fileService;

        [ObservableProperty]
        public partial ObservableCollection<EventDTO> Events { get; set; } = [];

        public EventsViewModel(IEventService eventService, IFileService fileService)
        {
            _eventService = eventService;
            _fileService = fileService;
        }

        private EventDTO? _selectedEvent;
        public EventDTO? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (SetProperty(ref _selectedEvent, value) && value != null)
                {
                    _ = ShowDetailsAsync(value.Id);
                    SetProperty(ref _selectedEvent, null);
                }
            }
        }

        [RelayCommand]
        private async Task InitAsync()
        {
            Events = new ObservableCollection<EventDTO>(await _eventService.GetAllAsync());
        }

        [RelayCommand]
        private async Task ShowDetailsAsync(int id)
        {
            await Shell.Current.GoToAsync($"{AppConstants.EventDetailRoute}?id={id}");
        }

        [RelayCommand]
        private async Task NewEventAsync()
        {
            await Shell.Current.GoToAsync(AppConstants.EventDetailRoute);
        }

        [RelayCommand]
        private async Task GoToDashboardAsync()
        {
            await Shell.Current.GoToAsync(AppConstants.DashboardRoute);
        }

        [RelayCommand]
        private async Task ExportEventsAsync()
        {
            if (!(await _fileService.ExportEventsAsync(Events)))
            {
                await Shell.Current.DisplayAlertAsync("Export Error", "No events exported", "OK");
            }
        }

        [RelayCommand]
        private async Task ImportEventsAsync()
        {
            var toImport = await _fileService.ImportEventsAsync();
            if (toImport == null)
            {
                await Shell.Current.DisplayAlertAsync("Import Error", "No events imported", "OK");
            }
            else
            {
                await _eventService.ImportAsync(toImport);
                Events = new ObservableCollection<EventDTO>(await _eventService.GetAllAsync());
            }
        }
    }
}
