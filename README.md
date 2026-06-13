# VodDownloader
基于 N_m3u8DL-RE 的资源站下载器

### test 命令
```cmd
.\VodDownload.exe test
```

检查 [ffmpeg](https://git.ffmpeg.org/ffmpeg.git) 和 [N_m3u8DL-RE](https://github.com/nilaoda/N_m3u8DL-RE) 是否可被调用

两者都可以放在本软件同目录或通过环境变量提供，建议 N_m3u8DL-RE 放在目录下，而 ffmpeg 通过环境变量提供

### list 命令
```cmd
.\VodDownload.exe list
```

输出当前支持的资源站列表

### url 命令
```cmd
.\VodDownload.exe url https://ffzy5.tv/index.php/vod/detail/id/29629.html
```

通过 URL 下载

### vid 命令
```cmd
.\VodDownload.exe vid ffzy 29629
```

通过视频 id 下载