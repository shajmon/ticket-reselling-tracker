using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class EventsViewModel : ObservableObject
    {
        private readonly IEventService _eventService;
        [ObservableProperty]
        public partial ObservableCollection<EventDTO> Events { get; set; } = [];

        public EventsViewModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        private EventDTO? _selectedEvent;
        public EventDTO? SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                if (SetProperty(ref _selectedEvent, value) && value != null)
                {
                    _ = ShowDetails(value.Id);
                    SetProperty(ref _selectedEvent, null);
                }
            }
        }

        [RelayCommand]
        private async Task Init()
        {
            Events = new ObservableCollection<EventDTO>(await _eventService.GetAllAsync());
        }

        [RelayCommand]
        private async Task ShowDetails(int id)
        {
            await Shell.Current.GoToAsync($"eventdetail?id={id}");
        }

        [RelayCommand]
        private async Task NewEvent()
        {
            await Shell.Current.GoToAsync("eventdetail");
        }

        [RelayCommand]
        private async Task GoToDashboard()
        {
            await Shell.Current.GoToAsync("//dashboard");
        }
    }
}
