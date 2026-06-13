using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VodDownloader.Entities;

public class VodListTemplate {
    [JsonPropertyName("code")]
    public Int32 Code{get;set;}

    [JsonPropertyName("msg")]
    public String Msg{get;set;} = String.Empty;

    [JsonPropertyName("page")]
    public Int32 Page{get;set;}

    [JsonPropertyName("pagecount")]
    public Int32 PageCount{get;set;}

    [JsonPropertyName("limit")]
    public String Limit{get;set;} = String.Empty;

    [JsonPropertyName("total")]
    public Int32 Total{get;set;}

    [JsonPropertyName("list")]
    public List<VodDetailTemplate> List{get;set;} = [];
}
