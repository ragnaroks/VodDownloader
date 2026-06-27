using System;
using System.Globalization;
using VodDownloader.Entities;

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

    public static (String type, UInt32 vid) ParseVid (Uri uri) {
        if (Constant.SiteTypeDictionary.TryGetValue(uri.IdnHost, out String? type) is false || String.IsNullOrWhiteSpace(type)) {
            return (String.Empty, 0);
        }
        if (type is "ffzy" or "lzzy" or "ikunzy" or "yhzy" or "bfzy") {
            String[] splitedPath = uri.AbsolutePath.Replace(".html", String.Empty).Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (splitedPath.Length < 1 || String.IsNullOrWhiteSpace(splitedPath[^1])) {
                return (type, 0);
            }
            if (UInt32.TryParse(splitedPath[^1], NumberStyles.Number, CultureInfo.InvariantCulture, out UInt32 vid) is false || vid < 1) {
                return (type, 0);
            }
            return (type, vid);
        } else {
            return (type, 0);
        }
    }
}
