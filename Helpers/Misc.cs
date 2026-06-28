using System;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using VodDownloader.Services.AppSetting;

namespace VodDownloader.Helpers;

public static class Misc {
    public static String FormatVodName (String value) {
        return String.IsNullOrWhiteSpace(value)
            ? "无标题"
            : value.Replace('/', '-')
                .Replace('\\', '-')
                .Replace(':', '：')
                .Replace('?', '？')
                .Replace('*', '◆')
                .Replace('\"', '″')
                .Replace('<', '《')
                .Replace('>', '》')
                .Replace('|', '-');
    }

    public static (String type, UInt32 vid) ParseTypeAndVid (Uri uri) {
        AppSettingService appSettingService = Program.ServiceProvider.GetRequiredService<AppSettingService>();
        String type = String.Empty;
        if (appSettingService.AppSettings.SiteType.FFZY.Contains(uri.IdnHost)) { type = "ffzy"; }
        if (appSettingService.AppSettings.SiteType.LZZY.Contains(uri.IdnHost)) { type = "lzzy"; }
        if (appSettingService.AppSettings.SiteType.IKUNZY.Contains(uri.IdnHost)) { type = "ikunzy"; }
        if (appSettingService.AppSettings.SiteType.YHZY.Contains(uri.IdnHost)) { type = "yhzy"; }
        if (appSettingService.AppSettings.SiteType.BFZY.Contains(uri.IdnHost)) { type = "bfzy"; }
        UInt32 vid = 0;
        switch (type) {
            case "ffzy":
            case "lzzy":
            case "ikunzy":
            case "yhzy":
            case "bfzy":
                String[] splitedPath = uri.AbsolutePath.Replace(".html", String.Empty).Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (splitedPath.Length < 1 || String.IsNullOrWhiteSpace(splitedPath[^1])) { break; }
                if (UInt32.TryParse(splitedPath[^1], NumberStyles.Number, CultureInfo.InvariantCulture, out vid) is false || vid < 1) { break; }
                break;
            default:
                break;
        }
        return (type, vid);
    }
}
