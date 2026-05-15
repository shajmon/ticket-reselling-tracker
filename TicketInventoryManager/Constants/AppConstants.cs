using System;
using System.Collections.Generic;
using System.Text;

namespace TicketInventoryManager.Constants
{
    public static class AppConstants
    {
        public const string DashboardRoute = "//dashboard";
        public const string LoginRoute = "//login";
        public const string InventoryRoute = "//inventory";
        public const string EventsRoute = "//events";
        public const string EventDetailRoute = "eventdetail";
        public const string LogDetailRoute = "logdetail";
        public const string DefaultExportNameEvents = "events_export.json";
        public const string DefaultExportNameLogs = "inventory_export.json";
        public const int MinUsernameLength = 3;
        public const int MinPasswordLength = 8;
    }
}
