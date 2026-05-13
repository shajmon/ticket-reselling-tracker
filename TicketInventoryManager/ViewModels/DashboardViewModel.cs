using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Models.Enums;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        public UserDTO User;
        private readonly IInventoryLogService _invLogService;
        private readonly ISessionService _sessionService;
        private readonly IEventService _eventService;
        private CancellationTokenSource _cts = new();
        public string Username => User.Username;

        [ObservableProperty]
        public partial DateTime FromSelector { get; set; } = DateTime.MinValue;

        [ObservableProperty]
        public partial DateTime ToSelector { get; set; } = DateTime.Now;
        [ObservableProperty]
        public partial ObservableCollection<EventDTO> Events { get; set; } = [];
        private EventDTO? _selectedEvent;
        public EventDTO? SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                if (SetProperty(ref _selectedEvent, value))
                {
                    EventId = value?.Id;
                    _ = LoadDataAsync();
                }
            }
        }

        [ObservableProperty]
        public partial int? EventId { get; set; }


        [ObservableProperty]
        public partial bool IsBusy { get; set; }


        [ObservableProperty]
        public partial int TicketsBought { get; set; }

        [ObservableProperty]
        public partial int UnsoldTickets { get; set; }

        [ObservableProperty]
        public partial decimal TotalSpent { get; set; }

        [ObservableProperty]
        public partial decimal AverageTicketBuyPrice { get; set; }

        [ObservableProperty]
        public partial decimal TotalUnsoldRetailValue { get; set; }


        [ObservableProperty]
        public partial int TicketsSold { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalRoi))]
        public partial decimal TotalRevenue { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalRoi))]
        public partial decimal TotalProfit { get; set; }

        public decimal TotalRoi => TotalRevenue == 0 ? 0 : TotalProfit / TotalRevenue * 100;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BestRoi))]
        public partial decimal BestProfit { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BestRoi))]
        public partial decimal BestEventSpend { get; set; }

        public decimal BestRoi => BestEventSpend == 0 ? 0 : BestProfit / BestEventSpend * 100;

        [ObservableProperty]
        public partial EventDTO? BestEvent { get; set; }

        public DashboardViewModel(IInventoryLogService inventoryLogService, ISessionService sessionService, IEventService eventService)
        {
            _invLogService = inventoryLogService;
            _sessionService = sessionService;
            _eventService = eventService;
            User = sessionService.CurrentUser!;
        }

        [RelayCommand]
        private void SelectThisMonth()
        {
            var now = DateTime.Now;
            FromSelector = new DateTime(now.Year, now.Month, 1);
            ToSelector = new DateTime(now.Year, now.Month, 1).AddMonths(1).AddDays(-1);
        }

        [RelayCommand]
        private void SelectLastMonth()
        {
            var now = DateTime.Now;
            FromSelector = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            ToSelector = new DateTime(now.Year, now.Month, 1).AddDays(-1);
        }

        [RelayCommand]
        private void SelectThisYear()
        {
            var now = DateTime.Now;
            FromSelector = new DateTime(now.Year, 1, 1);
            ToSelector = new DateTime(now.Year, 12, 31);
        }

        [RelayCommand]
        private async Task Init()
        {
            Events = new ObservableCollection<EventDTO>(await _eventService.GetAllAsync());
            await LoadDataAsync();
        }

        [RelayCommand]
        private void ClearEvent()
        {
            SelectedEvent = null;
        }

        [RelayCommand]
        private async Task Logout()
        {
            _sessionService.CurrentUser = null;
            await Shell.Current.GoToAsync("//login");
        }

        partial void OnFromSelectorChanged(DateTime value)
        {
            _ = LoadDataAsync();
        }

        partial void OnToSelectorChanged(DateTime value)
        {
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            
            try
            {
                await Task.Delay(50, _cts.Token);

                IsBusy = true;

                var summaryData = await _invLogService.GetSummaryAsync(User.Id, FromSelector, ToSelector, EventId);
                var buys = summaryData.Buys;
                var sells = summaryData.Sales;

                TicketsBought = buys.TicketsBought;
                UnsoldTickets = buys.UnsoldTickets;
                TotalSpent = buys.TotalSpent;
                AverageTicketBuyPrice = buys.AverageTicketBuyPrice;
                TotalUnsoldRetailValue = buys.TotalUnsoldRetailValue;

                TicketsSold = sells.TicketsSold;
                TotalRevenue = sells.TotalRevenue;
                TotalProfit = sells.TotalProfit;
                BestProfit = sells.BestProfit;
                BestEventSpend = sells.BestEventSpend;
                BestEvent = sells.BestEvent;
            }
            catch (OperationCanceledException) { }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToInventory()
        {
            await Shell.Current.GoToAsync("//inventory");
        }

        [RelayCommand]
        private async Task GoToEvents()
        {
            await Shell.Current.GoToAsync("//events");
        }
    }
}
