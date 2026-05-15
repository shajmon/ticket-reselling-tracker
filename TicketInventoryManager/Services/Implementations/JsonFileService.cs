using CommunityToolkit.Maui.Storage;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using TicketInventoryManager.Constants;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public class JsonFileService : IFileService
    {
        
        
        public async Task<bool> ExportEventsAsync(ObservableCollection<EventDTO> events)
        {
            return await ExportAsync(events, AppConstants.DefaultExportNameEvents);
        }

        public async Task<bool> ExportLogsAsync(ObservableCollection<InventoryLogDTO> logs)
        {
            return await ExportAsync(logs, AppConstants.DefaultExportNameLogs);
        }

        public async Task<ObservableCollection<EventDTO>?> ImportEventsAsync()
        {
            return await ImportAsync<EventDTO>();
        }

        public async Task<ObservableCollection<InventoryLogDTO>?> ImportLogsAsync()
        {
            return await ImportAsync<InventoryLogDTO>();
        }

        private async Task<bool> ExportAsync<T>(ObservableCollection<T> list, string defaultName)
        {
            string serialized = JsonSerializer.Serialize(list);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(serialized));
            try
            {
                await FileSaver.Default.SaveAsync(defaultName, stream);
                return true;
            }
            catch { }
            return false;
        }

        private async Task<ObservableCollection<T>?> ImportAsync<T>()
        {
            var selected = await FilePicker.Default.PickAsync();
            if (selected == null)
            {
                return null;
            }

            using var stream = await selected.OpenReadAsync();
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            try
            {
                return JsonSerializer.Deserialize<ObservableCollection<T>>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
