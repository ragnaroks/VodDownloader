using System;

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
}
