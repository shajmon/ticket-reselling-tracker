using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TicketInventoryManager.Constants;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial UserDTO User {get; set;}
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
        [NotifyPropertyChangedFor(nameof(IsTotalRoiLoss))]
        public partial decimal TotalRevenue { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TotalRoi))]
        [NotifyPropertyChangedFor(nameof(IsTotalProfitLoss))]
        [NotifyPropertyChangedFor(nameof(IsTotalRoiLoss))]
        public partial decimal TotalProfit { get; set; }

        public decimal TotalRoi => TotalRevenue == TotalProfit ? 0 : TotalProfit / (TotalRevenue - TotalProfit) * 100;
        public bool IsTotalProfitLoss => TotalProfit < 0;
        public bool IsTotalRoiLoss => TotalRoi < 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BestRoi))]
        [NotifyPropertyChangedFor(nameof(IsBestProfitLoss))]
        [NotifyPropertyChangedFor(nameof(IsBestRoiLoss))]
        public partial decimal BestProfit { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BestRoi))]
        [NotifyPropertyChangedFor(nameof(IsBestRoiLoss))]
        public partial decimal BestEventSpend { get; set; }

        public decimal BestRoi => BestEventSpend == 0 ? 0 : BestProfit / BestEventSpend * 100;
        public bool IsBestProfitLoss => BestProfit < 0;
        public bool IsBestRoiLoss => BestRoi < 0;

        [ObservableProperty]
        public partial EventDTO? BestEvent { get; set; }

        public DashboardViewModel(IInventoryLogService inventoryLogService,
                                  ISessionService sessionService, 
                                  IEventService eventService)
        {
            _invLogService = inventoryLogService;
            _sessionService = sessionService;
            _eventService = eventService;
            User = sessionService.CurrentUser!;
        }

        [RelayCommand]
        private void SelectAllTime()
        {
            FromSelector = DateTime.MinValue;
            ToSelector = DateTime.Now;
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
        private async Task InitAsync()
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
        private async Task LogoutAsync()
        {
            _sessionService.CurrentUser = null;
            await Shell.Current.GoToAsync(AppConstants.LoginRoute);
        }

        [RelayCommand]
        private async Task GoToInventoryAsync()
        {
            await Shell.Current.GoToAsync(AppConstants.InventoryRoute);
        }

        [RelayCommand]
        private async Task GoToEventsAsync()
        {
            await Shell.Current.GoToAsync(AppConstants.EventsRoute);
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

                if (IsBusy) return;
                IsBusy = true;

                var summaryData = await _invLogService.GetSummaryAsync(User.Id, FromSelector,
                                                                       ToSelector, EventId);
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
    }
}
