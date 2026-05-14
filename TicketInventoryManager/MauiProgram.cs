using CommunityToolkit.Maui;
using DAL;
using Microsoft.Extensions.Logging;
using TicketInventoryManager.Services;

namespace TicketInventoryManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddTransient<AppDbContext>();

            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddTransient<IEventService, EventService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IInventoryLogService, InventoryLogService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddTransient<IFileService, JsonFileService>();

            builder.Services.AddTransient<Views.LoginPage>();
            builder.Services.AddTransient<Views.DashboardPage>();
            builder.Services.AddTransient<Views.InventoryLogsPage>();
            builder.Services.AddTransient<Views.InventoryLogDetailsPage>();
            builder.Services.AddTransient<Views.EventsPage>();
            builder.Services.AddTransient<Views.EventDetailsPage>();

            builder.Services.AddTransient<ViewModels.LoginViewModel>();
            builder.Services.AddTransient<ViewModels.DashboardViewModel>();
            builder.Services.AddTransient<ViewModels.InventoryLogViewModel>();
            builder.Services.AddTransient<ViewModels.InventoryLogDetailsViewModel>();
            builder.Services.AddTransient<ViewModels.EventsViewModel>();
            builder.Services.AddTransient<ViewModels.EventDetailsViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureDeleted();
            scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();

            return app;
        }
    }
}
