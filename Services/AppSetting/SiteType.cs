using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VodDownloader.Services.AppSetting;

public sealed class SiteType {
    [JsonPropertyName("ffzy")]
    public List<String> FFZY { get; set; } = [];

    [JsonPropertyName("lzzy")]
    public List<String> LZZY { get; set; } = [];

    [JsonPropertyName("ikunzy")]
    public List<String> IKUNZY { get; set; } = [];

    [JsonPropertyName("yhzy")]
    public List<String> YHZY { get; set; } = [];

    [JsonPropertyName("bfzy")]
    public List<String> BFZY { get; set; } = [];
}
