using System.Text.Json.Serialization;

namespace VodDownloader.Services.AppSetting;

public sealed class Settings {
    [JsonPropertyName("site-type")]
    public SiteType SiteType { get; set; } = new();
}
