using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Enums;
using System;
using System.Collections.Generic;
using TicketInventoryManager.Models.Entities;
using TicketInventoryManager.Models.Enums;
using TicketInventoryManager.Services;

namespace TicketInventoryManager.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private UserDTO _user;
        private readonly IInventoryLogService _invLogService;
        public string Username => _user.Username;

        [ObservableProperty]
        public partial DateTime? FromSelector { get; set; }

        [ObservableProperty]
        public partial DateTime? ToSelector { get; set; }

        [ObservableProperty]
        public partial HashSet<ItemStatus> StatusFilter { get; set; }

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
        public partial decimal TotalRevenue { get; set; }

        [ObservableProperty]
        public partial decimal TotalProfit { get; set; }

        [ObservableProperty]
        public partial decimal TotalRoi { get; set; }

        [ObservableProperty]
        public partial decimal BestProfit { get; set; }

        [ObservableProperty]
        public partial decimal BestRoi { get; set; }

        [ObservableProperty]
        public partial TimeSpan AverageHoldTime { get; set; }

        [ObservableProperty]
        public partial EventDTO BestEvent { get; set; }

        public DashboardViewModel(IInventoryLogService inventoryLogService)
        {
            _invLogService = inventoryLogService;
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

        private async Task LoadDataAsync()
        {

        }
    }
}
