using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using VodDownloader.Entities;
using VodDownloader.Interfaces;

namespace VodDownloader.Services.IKUNZYParser;

public sealed class IKUNZYParserService : ISiteParserService {
    private String UrlPath { get; } = "/api.php/provide/vod/at/json/ac/detail/ids/";
    private HttpClient HttpClient { get; }

    public IKUNZYParserService (HttpClient httpClient) {
        this.HttpClient = httpClient;
    }

    public VodDetailLite? GetVodDetail (UInt32 vid) {
        String httpBody;
        try {
            ReadOnlySpan<Byte> httpBytes = this.HttpClient.GetByteArrayAsync(String.Concat(this.UrlPath, vid)).GetAwaiter().GetResult();
            httpBody = Encoding.UTF8.GetString(httpBytes);
        } catch (Exception ex) {
            Console.WriteLine("[IKUNZYParserService] HTTP 请求失败");
            Console.WriteLine(ex.Message);
            return null;
        }
        if (String.IsNullOrWhiteSpace(httpBody)) {
            Console.WriteLine("[IKUNZYParserService] 获取数据失败，响应为空");
            return null;
        }
        JsonDetailResponse? response;
        try {
            response = JsonSerializer.Deserialize<JsonDetailResponse>(httpBody);
        } catch (Exception ex) {
            Console.WriteLine("[IKUNZYParserService] 解析数据失败");
            Console.WriteLine(ex.Message);
            return null;
        }
        if (response is null || response.Code is not 1) {
            Console.WriteLine("[IKUNZYParserService] 解析数据失败");
            return null;
        }
        if (response.List.Count < 1) {
            Console.WriteLine("[IKUNZYParserService] 剧集为空");
            return null;
        }
        VodDetailTemplate vodDetail = response.List[0];
        if (vodDetail is null) {
            Console.WriteLine("[IKUNZYParserService] 剧集为空");
            return null;
        }
        String[] splitedArray = vodDetail.VodPlayUrl.Split('#', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (splitedArray.Length < 1) {
            Console.WriteLine("[IKUNZYParserService] 剧集为空");
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
