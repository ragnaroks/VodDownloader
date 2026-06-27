using System;
using System.Text.Json.Serialization;
using VodDownloader.Entities;

namespace VodDownloader.Services.BFZYParser;

public sealed class JsonDetailResponse : VodListTemplate {
    [JsonPropertyName("limit")]
    public new Int32 Limit{get;set;} = 0;
}
