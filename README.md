# Ticket Inventory Manager

A cross-platform .NET MAUI app for tracking ticket flips - buying and reselling event tickets for profit. Log purchases, mark them sold, and watch profit, ROI, and per-event performance on a filterable dashboard.

## Tech stack

- **.NET 10** - targets Windows, Android, iOS, macCatalyst
- **.NET MAUI** with MVVM (CommunityToolkit.Mvvm)
- **EF Core + SQLite** for persistence (DAL class library)
- **BCrypt.Net-Next** for password hashing
- **CommunityToolkit.Maui** for file save/pick (JSON import/export)

## Prerequisites

- **.NET 10 SDK**
- **.NET MAUI workload** - install once with:

  ```bash
  dotnet workload install maui
  ```

- **Windows 10 19041 or newer** (primary target). Visual Studio 2022/2026 with the ".NET Multi-platform App UI development" workload is recommended but not required.

NuGet packages are restored automatically by `dotnet build` / `dotnet restore`. The full list (versions from the `.csproj` files):

**`DAL` project**
- `BCrypt.Net-Next` 4.1.0
- `Microsoft.EntityFrameworkCore.Sqlite` 10.0.7
- `Microsoft.Data.Sqlite` 10.0.7

**`TicketInventoryManager` project**
- `BCrypt.Net-Next` 4.1.0
- `Microsoft.EntityFrameworkCore.Sqlite` 10.0.7
- `Microsoft.Data.Sqlite` 10.0.7
- `CommunityToolkit.Mvvm` 8.4.2
- `CommunityToolkit.Maui` 14.1.1
- `Microsoft.Maui.Controls` 10.0.60
- `Microsoft.Extensions.Logging.Debug` 10.0.7

## Project layout

```
TicketInventoryManager.slnx              # solution file
│
├── DAL/                                  # class library: EF Core data access
│   ├── DAL.csproj
│   ├── AppDbContext.cs                   # DbContext, OnConfiguring, OnModelCreating + seed
│   ├── Entities/
│   │   ├── User.cs                       # Id, Username, PasswordHash, IsAdmin
│   │   ├── Event.cs                      # Name, VenueName, City, Country, Date, EventType
│   │   └── InventoryLog.cs               # buy/sell info, qty, prices, platforms, status
│   └── Enums/
│       ├── EventType.cs                  # Concert, Sports, Theatre
│       └── ItemStatus.cs                 # NotListed, Listed, ToDeliver, Delivered
│
└── TicketInventoryManager/               # .NET MAUI app
    ├── TicketInventoryManager.csproj
    ├── MauiProgram.cs                    # bootstrap, DI container, font registration
    ├── App.xaml / App.xaml.cs            # app entry, sets AppShell as window root
    ├── AppShell.xaml / AppShell.xaml.cs  # shell navigation, route registration
    │
    ├── Constants/
    │   └── AppConstants.cs               # route names, validation limits, file names
    │
    ├── Models/
    │   ├── Entities/                     # DTOs exposed to the UI
    │   │   ├── UserDTO.cs
    │   │   ├── EventDTO.cs
    │   │   └── InventoryLogDTO.cs        # + computed Profit, Roi, DaysHeld
    │   └── DataSummary/                  # dashboard aggregate records
    │       ├── DashboardSummary.cs
    │       ├── BuysSummary.cs
    │       └── SalesSummary.cs
    │
    ├── Services/
    │   ├── Interfaces/                   # ISessionService, IUserService,
    │   │                                 # IEventService, IInventoryLogService, IFileService
    │   └── Implementations/              # SessionService, UserService, EventService,
    │                                     # InventoryLogService, JsonFileService
    │
    ├── ViewModels/                       # one VM per page
    │   ├── LoginViewModel.cs
    │   ├── DashboardViewModel.cs
    │   ├── EventsViewModel.cs
    │   ├── EventDetailsViewModel.cs
    │   ├── InventoryLogViewModel.cs
    │   └── InventoryLogDetailsViewModel.cs
    │
    ├── Views/                            # XAML pages + code-behind
    │   ├── LoginPage.xaml(.cs)
    │   ├── DashboardPage.xaml(.cs)
    │   ├── EventsPage.xaml(.cs)
    │   ├── EventDetailsPage.xaml(.cs)
    │   ├── InventoryLogsPage.xaml(.cs)
    │   └── InventoryLogDetailsPage.xaml(.cs)
    │
    ├── Resources/                        # Styles, Colors, Fonts, Images, AppIcon, Splash
    ├── Platforms/                        # Windows / Android / iOS / macCatalyst entry points
    └── Properties/                       # launchSettings.json
```

## Build & run

```bash
# Restore packages (also runs implicitly with build)
dotnet restore

# Build the whole solution
dotnet build

# Run
dotnet run --project TicketInventoryManager
```

Schema is generated on first run via `Database.EnsureCreated()` - no migrations needed.

## Features

- **Login & register** - BCrypt-hashed passwords, session held in a singleton service
- **Dashboard** - buys / sales split, date-range and event filters, presets (this month, last month, this year, all time), best-event highlight, profit & ROI
- **Inventory logs** - list with status filter (NotListed / Listed / ToDeliver / Delivered), add / edit / delete, JSON import & export (replace or append)
- **Events** - shared event catalog, admin-only create/edit/delete, JSON import & export
- **Per-user data** - each user owns their own logs; events are shared

## Business rules

- Each user owns their own `InventoryLog`s; `Event`s are shared across all users
- Only admin users can create/edit/delete events and import/export the event catalog
- `EventType` is immutable after creation
- Passwords are never stored in plain text
- Services own the entity ↔ DTO conversion; ViewModels never touch entities directly

## Architecture notes

```
Views (XAML) ↔ ViewModels ↔ Services ↔ DAL (EF Core / SQLite)
```

- Shell navigation with `//` prefix at the auth boundary (clears back stack); detail pages use `Routing.RegisterRoute` for a normal back stack and `[QueryProperty]` parameters.
- Dashboard aggregates run in SQL via `IQueryable` (not in memory).
- SQLite's EF provider can't always parse `decimal` arithmetic - projections cast to `double` then back to `decimal`.

## AI usage declaration

AI assistance (Claude Code) was used during development for the following:

- Drafting this README
- Assistance with EF Core / SQLite query design (dashboard aggregates, the `decimal` → `double` cast workaround)
- Convention and consistency checks across ViewModels, Services, and XAML bindings
- Dead code lookup and cleanup passes
- Consultations on overall project structure (DAL/MAUI separation, DI wiring, MVVM patterns, navigation)
- UI design - layout decisions, dark theme styling, and dashboard section composition
