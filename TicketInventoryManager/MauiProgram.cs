using DAL;
using Microsoft.Extensions.Logging;

namespace TicketInventoryManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var context = new AppDbContext();
            context.Database.EnsureCreated();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
