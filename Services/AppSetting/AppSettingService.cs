using System;
using System.IO;
using System.Text.Json;

namespace VodDownloader.Services.AppSetting;

public sealed class AppSettingService {
    public Settings AppSettings { get; private set; }

    public AppSettingService () {
        String filepath = Path.Combine(AppContext.BaseDirectory, "settings.json");
        try {
            Span<Byte> bytes = File.ReadAllBytes(filepath);
            this.AppSettings = JsonSerializer.Deserialize<Settings>(bytes) ?? new();
        } catch {
            throw;
        }
    }
}
