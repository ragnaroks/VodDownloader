using System;
using System.Net.Http;
using System.Text.Json;
using VodDownloader.Entities;
using VodDownloader.Interfaces;

namespace VodDownloader.Services.LZZYParser;

public sealed class LZZYParserService : ISiteParserService {
    private String UrlPath { get; } = "/api.php/provide/vod/from/lzm3u8/at/json/ac/detail/ids/";
    private HttpClient HttpClient { get; }

    public LZZYParserService (HttpClient httpClient) {
        this.HttpClient = httpClient;
    }

    public VodDetailLite? GetVodDetail (UInt32 vid) {
        String httpBody;
        try {
            httpBody = this.HttpClient.GetStringAsync(String.Concat(this.UrlPath, vid)).GetAwaiter().GetResult();
        } catch (Exception ex) {
            Console.WriteLine("[LZZYParserService] HTTP 请求失败");
            Console.WriteLine(ex.Message);
            return null;
        }
        if (String.IsNullOrWhiteSpace(httpBody)) {
            Console.WriteLine("[LZZYParserService] 获取数据失败，响应为空");
            return null;
        }
        JsonDetailResponse? response;
        try {
            response = JsonSerializer.Deserialize<JsonDetailResponse>(httpBody);
        } catch (Exception ex) {
            Console.WriteLine("[LZZYParserService] 解析数据失败");
            Console.WriteLine(ex.Message);
            return null;
        }
        if (response is null || response.Code is not 1) {
            Console.WriteLine("[LZZYParserService] 解析数据失败");
            return null;
        }
        if (response.List.Count < 1) {
            Console.WriteLine("[LZZYParserService] 剧集为空");
            return null;
        }
        VodDetailTemplate vodDetail = response.List[0];
        if (vodDetail is null) {
            Console.WriteLine("[LZZYParserService] 剧集为空");
            return null;
        }
        String[] splitedArray = vodDetail.VodPlayUrl.Split('#', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (splitedArray.Length < 1) {
            Console.WriteLine("[LZZYParserService] 剧集为空");
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
