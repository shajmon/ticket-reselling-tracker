using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    [QueryProperty(nameof(EventId), "id")]
    public partial class EventDetailsViewModel : ObservableObject
    {
        private readonly IEventService _eventService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExistingEvent))]
        [NotifyPropertyChangedFor(nameof(IsNewEvent))]
        public partial int EventId { get; set; }
        public bool IsExistingEvent => EventId != 0;
        public bool IsNewEvent => EventId == 0;

        public IList<EventType> EventTypes { get; } = Enum.GetValues<EventType>().ToList();

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string VenueName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string City { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Country { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTime Date { get; set; } = DateTime.Today;

        [ObservableProperty]
        public partial EventType EventType { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        public partial string? ErrorMessage { get; set; }
        public bool HasError => ErrorMessage != null;

        public EventDetailsViewModel(IEventService eventService)
        {
            _eventService = eventService;
        }

        [RelayCommand]
        private async Task InitAsync()
        {
            if (!IsExistingEvent) return;

            var ev = await _eventService.GetByIdAsync(EventId);
            if (ev == null) return;

            Name = ev.Name;
            VenueName = ev.VenueName;
            City = ev.City;
            Country = ev.Country;
            Date = ev.Date;
            EventType = ev.EventType;
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (!IsValidUserInput())
            {
                return;
            }

            ErrorMessage = null;

            if (IsExistingEvent)
                await _eventService.UpdateAsync(GetDTOFromProperties());
            else
                await _eventService.AddAsync(GetDTOFromProperties());

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            await _eventService.DeleteAsync(EventId);
            await Shell.Current.GoToAsync("..");
        }

        private EventDTO GetDTOFromProperties()
        {
            return new EventDTO
            {
                Id = EventId,
                Name = Name,
                VenueName = VenueName,
                City = City,
                Country = Country,
                Date = Date,
                EventType = EventType
            };
        }

        private bool IsValidUserInput()
        {
            if (string.IsNullOrWhiteSpace(Name))
            { 
                ErrorMessage = "Name is required"; 
                return false; 
            }
            if (string.IsNullOrWhiteSpace(VenueName))
            { 
                ErrorMessage = "Venue is required";
                return false; 
            }
            if (string.IsNullOrWhiteSpace(City))
            { 
                ErrorMessage = "City is required";
                return false; 
            }
            if (string.IsNullOrWhiteSpace(Country))
            { 
                ErrorMessage = "Country is required";
                return false; 
            }
            return true;
        }
    }
}
