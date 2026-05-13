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

        public int LogId { get; set; }
        public bool IsExistingLog => LogId != 0;

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
        public partial int Quantity { get; set; } = 1;

        [ObservableProperty]
        public partial decimal BuyPerOne { get; set; }

        [ObservableProperty]
        public partial decimal SellPerOne { get; set; }

        [ObservableProperty]
        public partial string BuyPlatform { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string AccountEmail { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string SellPlatform { get; set; } = string.Empty;

        [ObservableProperty]
        public partial ItemStatus Status { get; set; }

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
                Quantity = log.Quantity;
                BuyPerOne = log.BuyPerOne;
                SellPerOne = log.SellPerOne ?? 0;
                BuyPlatform = log.BuyPlatform;
                AccountEmail = log.AccountEmail;
                SellPlatform = log.SellPlatform;
                Status = log.Status;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
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
                Quantity = Quantity,
                BuyPerOne = BuyPerOne,
                SellPerOne = IsSold ? SellPerOne : null,
                BuyPlatform = BuyPlatform,
                AccountEmail = AccountEmail,
                SellPlatform = SellPlatform,
                Status = Status
            };
        }
    }
}
