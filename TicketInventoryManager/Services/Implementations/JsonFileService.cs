using CommunityToolkit.Maui.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using TicketInventoryManager.Models.Entities;

namespace TicketInventoryManager.Services
{
    public class JsonFileService : IFileService
    {
        public async Task<bool> ExportEventsAsync(ObservableCollection<EventDTO> events)
        {
            string serializedEvents = JsonSerializer.Serialize(events);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(serializedEvents));
            try
            {
                await FileSaver.Default.SaveAsync("events_export.json", stream);
                return true;
            }
            catch { }
            return false;
        }

        public async Task<bool> ExportLogsAsync(ObservableCollection<InventoryLogDTO> logs)
        {
            string json = JsonSerializer.Serialize(logs);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            try
            {
                await FileSaver.Default.SaveAsync("logs_export.json", stream);
                return true;
            }
            catch { }
            return false;
        }

        public async Task<ObservableCollection<EventDTO>?> ImportEventsAsync()
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
                return JsonSerializer.Deserialize<ObservableCollection<EventDTO>>(json);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ObservableCollection<InventoryLogDTO>?> ImportLogsAsync()
        {
            var selected = await FilePicker.Default.PickAsync();
            if (selected == null) return null;

            using var stream = await selected.OpenReadAsync();
            using var reader = new StreamReader(stream);
            string json = await reader.ReadToEndAsync();

            try
            {
                return JsonSerializer.Deserialize<ObservableCollection<InventoryLogDTO>>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
