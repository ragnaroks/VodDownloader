using System;
using System.Net.Http;
using System.Text.Json;
using VodDownloader.Entities;
using VodDownloader.Interfaces;

namespace VodDownloader.Services.BFZYParser;

public sealed class BFZYParserService : ISiteParserService {
    private String UrlPath { get; } = "/api.php/provide/vod/at/json/ac/detail/ids/";

    private HttpClient HttpClient { get; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:使用主构造函数", Justification = "<挂起>")]
    public BFZYParserService (HttpClient httpClient) {
        this.HttpClient = httpClient;
    }

    public VodDetailLite? GetVodDetail (UInt32 vid) {
        String httpBody;
        try {
            httpBody = this.HttpClient.GetStringAsync($"{this.UrlPath}{vid}").GetAwaiter().GetResult();
        } catch (Exception ex) {
            Console.WriteLine("[BFZYParserService] HTTP 请求失败");
            Console.WriteLine(ex.Message);
            return null;
        }
        if (String.IsNullOrWhiteSpace(httpBody)) {
            Console.WriteLine("[BFZYParserService] 获取数据失败，响应为空");
            return null;
        }
        JsonDetailResponse? response;
        try {
            response = JsonSerializer.Deserialize<JsonDetailResponse>(httpBody);
        } catch (Exception ex) {
            Console.WriteLine("[BFZYParserService] 解析数据失败");
            Console.WriteLine(ex.Message);
            return null;
        }
        if (response is null || response.Code is not 1) {
            Console.WriteLine("[BFZYParserService] 解析数据失败");
            return null;
        }
        if (response.List.Count < 1) {
            Console.WriteLine("[BFZYParserService] 剧集为空");
            return null;
        }
        VodDetailTemplate vodDetail = response.List[0];
        if (vodDetail is null) {
            Console.WriteLine("[BFZYParserService] 剧集为空");
            return null;
        }
        String[] splitedArray = vodDetail.VodPlayUrl.Split('#', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (splitedArray.Length < 1) {
            Console.WriteLine("[BFZYParserService] 剧集为空");
            return null;
        }
        VodDetailLite vodDetailLite = new() {
            Id = Convert.ToUInt32(vodDetail.VodId),
            Name = vodDetail.VodName,
            Cover = vodDetail.VodPic,
            Date = String.IsNullOrWhiteSpace(vodDetail.VodPubdate) ? vodDetail.VodYear : vodDetail.VodPubdate,
            Description = vodDetail.VodContent
        };
        foreach (String item in splitedArray) {
            if (item.EndsWith(".m3u8") is false) { continue; }
            vodDetailLite.M3U8Queue.Enqueue(item);
        }
        return vodDetailLite;
    }
}
