using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using System.Collections.ObjectModel;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    [QueryProperty(nameof(LogId), "id")]
    public partial class InventoryLogDetailsViewModel : ObservableObject
    {
        private readonly IInventoryLogService _invLogService;
        private readonly ISessionService _sessionService;
        private readonly IEventService _eventService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsExistingLog))]
        public partial int LogId { get; set; }
        public bool IsExistingLog => LogId != 0;
        public IList<ItemStatus> Statuses { get; } = Enum.GetValues<ItemStatus>().ToList();

        [ObservableProperty]
        public partial ObservableCollection<EventDTO> Events { get; set; } = [];

        private EventDTO? _selectedEvent;
        public EventDTO? SelectedEvent
        {
            get => _selectedEvent;
            set => SetProperty(ref _selectedEvent, value);
        }

        [ObservableProperty]
        public partial DateTime BuyDate { get; set; } = DateTime.Today;

        [ObservableProperty]
        public partial bool IsSold { get; set; }

        [ObservableProperty]
        public partial DateTime SellDate { get; set; } = DateTime.Today;

        [ObservableProperty]
        public partial string Sector { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Quantity { get; set; } = "1";

        [ObservableProperty]
        public partial string BuyPerOne { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string SellPerOne { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string BuyPlatform { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string AccountEmail { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string SellPlatform { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ItemStatus Status { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasError))]
        public partial string? ErrorMessage { get; set; }
        public bool HasError => ErrorMessage != null;

        public InventoryLogDetailsViewModel(IInventoryLogService invLogService, ISessionService sessionService, IEventService eventService)
        {
            _invLogService = invLogService;
            _sessionService = sessionService;
            _eventService = eventService;
        }

        [RelayCommand]
        private async Task Init()
        {
            Events = new ObservableCollection<EventDTO>(await _eventService.GetAllAsync());

            if (IsExistingLog)
            {
                var log = await _invLogService.GetByIdAsync(LogId);
                if (log == null) return;

                SelectedEvent = Events.FirstOrDefault(e => e.Id == log.EventId);
                BuyDate = log.BuyDate;
                IsSold = log.SellDate.HasValue;
                SellDate = log.SellDate ?? DateTime.Today;
                Sector = log.Sector;
                Quantity = log.Quantity.ToString();
                BuyPerOne = log.BuyPerOne.ToString();
                SellPerOne = (log.SellPerOne ?? 0).ToString();
                BuyPlatform = log.BuyPlatform;
                AccountEmail = log.AccountEmail;
                SellPlatform = log.SellPlatform;
                Status = log.Status;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            if (!IsValidUserInput())
            {
                return;
            }

            ErrorMessage = null;

            if (IsExistingLog)
                await _invLogService.UpdateAsync(GetDTOFromProperties());
            else
                await _invLogService.AddAsync(GetDTOFromProperties());

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task Delete()
        {
            await _invLogService.DeleteAsync(LogId);
            await Shell.Current.GoToAsync("..");
        }

        private InventoryLogDTO GetDTOFromProperties()
        {
            return new InventoryLogDTO
            {
                Id = LogId,
                UserId = _sessionService.CurrentUser!.Id,
                EventId = SelectedEvent?.Id ?? 0,
                BuyDate = BuyDate,
                SellDate = IsSold ? SellDate : null,
                Sector = Sector,
                Quantity = int.TryParse(Quantity, out var qty) ? qty : 0,
                BuyPerOne = decimal.TryParse(BuyPerOne, out var buy) ? buy : 0,
                SellPerOne = IsSold && decimal.TryParse(SellPerOne, out var sell) ? sell : null,
                BuyPlatform = BuyPlatform,
                AccountEmail = AccountEmail,
                SellPlatform = SellPlatform,
                Status = Status
            };
        }

        private bool IsValidUserInput()
        {
            if (SelectedEvent == null)
            { 
                ErrorMessage = "Please select an event"; 
                return false; 
            }
            if (string.IsNullOrWhiteSpace(Sector))
            { 
                ErrorMessage = "Sector is required";
                return false; 
            }
            if (!int.TryParse(Quantity, out var qty) || qty <= 0)
            { 
                ErrorMessage = "Quantity must be a positive number";
                return false; 
            }
            if (!decimal.TryParse(BuyPerOne, out var buy) || buy <= 0)
            { 
                ErrorMessage = "Buy price must be a positive number";
                return false; 
            }
            if (string.IsNullOrWhiteSpace(BuyPlatform))
            { 
                ErrorMessage = "Buy platform is required";
                return false; 
            }
            if (string.IsNullOrWhiteSpace(AccountEmail))
            { 
                ErrorMessage = "Account email is required";
                return false; 
            }
            if (IsSold)
            {
                if (!decimal.TryParse(SellPerOne, out var sell) || sell <= 0)
                { 
                    ErrorMessage = "Sell price must be a positive number";
                    return false; 
                }
                if (string.IsNullOrWhiteSpace(SellPlatform))
                { 
                    ErrorMessage = "Sell platform is required";
                    return false; 
                }
            }
            return true;
        }
    }
}
