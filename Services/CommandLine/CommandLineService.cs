using System;
using System.CommandLine;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using VodDownloader.Entities;
using VodDownloader.Helpers;
using VodDownloader.Interfaces;

namespace VodDownloader.Services.CommandLine;

public sealed class CommandLineService {
    public RootCommand RootCommand { get; private set; }

    private Command SubCommandTest { get; } = new("test");

    private Command SubCommandURL { get; } = new("url");

    private Argument<String> ArgumentURL { get; } = new("URL");

    private Command SubCommandVid { get; } = new("vid");

    private Argument<String> ArgumentType { get; } = new("TYPE");

    private Argument<UInt32> ArgumentVid { get; } = new("VID");

    private Command SubCommandList { get; } = new("list");

    private Option<Boolean> OptionDev { get; } = new("--dev");

    private Option<UInt32> OptionThreadCount { get; } = new("--thread-count");

    public CommandLineService () {
        this.OptionDev.Description = "是否启用开发者模式";

        this.OptionThreadCount.Description = "并发下载分片数量，部分站点会降速或卡住，设置较低的数值（比如 4）会有用";
        this.OptionThreadCount.Aliases.Add("--tc");
        this.OptionThreadCount.DefaultValueFactory = result => 32;
        this.OptionThreadCount.Validators.Add(result => {
            if (result.GetValue<UInt32>(this.OptionThreadCount) <= 32) { return; }
            result.AddError("选项 --thread-count 的值不能超过 32");
        });

        this.SubCommandTest.SetAction(this.CommandTest);

        this.ArgumentURL.Description = "资源库视频详情页面地址";
        this.ArgumentURL.Validators.Add((result) => {
            String value = result.GetValueOrDefault<String>();
            Boolean valid = Uri.TryCreate(value, UriKind.Absolute, out Uri? newValue);
            if (valid is true && newValue is not null) { return; }
            result.AddError("参数 <URL> 错误，反序列化失败");
        });
        this.SubCommandURL.SetAction(this.CommandURL);
        this.SubCommandURL.Arguments.Add(this.ArgumentURL);
        this.SubCommandURL.Options.Add(this.OptionThreadCount);

        this.ArgumentType.Description = "资源库类型，例如 ffzy、lzzy 等 ，详情请执行 list 子命令查看";
        this.ArgumentVid.Description = "视频 id，例如 114514 等";
        this.SubCommandVid.SetAction(this.CommandVid);
        this.SubCommandVid.Arguments.Add(this.ArgumentType);
        this.SubCommandVid.Arguments.Add(this.ArgumentVid);
        this.SubCommandVid.Options.Add(this.OptionThreadCount);

        this.SubCommandList.SetAction(this.CommandList);

        this.RootCommand = new("VodDownloader");
        this.RootCommand.Options.Add(this.OptionDev);
        this.RootCommand.Subcommands.Add(this.SubCommandTest);
        this.RootCommand.Subcommands.Add(this.SubCommandURL);
        this.RootCommand.Subcommands.Add(this.SubCommandVid);
        this.RootCommand.Subcommands.Add(this.SubCommandList);
    }

    private void CommandTest (ParseResult parseResult) {
        Process? process = null;
        try {
            process = Process.Start(new ProcessStartInfo() {
                UseShellExecute = false,
                FileName = "ffmpeg",
                Arguments = "-version",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            });
        } catch (Exception ex) {
            process?.Dispose();
            Console.WriteLine("[×] ffmpeg 检测失败，以下是输出详情");
            Console.WriteLine(ex.Message);
            return;
        }
        if (process is null) {
            Console.WriteLine("[×] ffmpeg 检测失败，未找到可执行文件");
            return;
        }
        process.WaitForExit();
        String output = process.StandardOutput.ReadToEnd();
        process.Dispose();
        if (output.Contains("ffmpeg version") && output.Contains("exit code 0")) {
            Console.WriteLine("[√] ffmpeg 检测通过");
        } else {
            Console.WriteLine("[×] ffmpeg 检测失败，以下是输出详情");
            Console.WriteLine(output);
        }

        Process? process2 = null;
        try {
            process2 = Process.Start(new ProcessStartInfo() {
                UseShellExecute = false,
                FileName = "N_m3u8DL-RE",
                Arguments = "--version",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            });
        } catch (Exception ex) {
            process2?.Dispose();
            Console.WriteLine("[×] N_m3u8DL-RE 检测失败，以下是输出详情");
            Console.WriteLine(ex.Message);
            return;
        }
        if (process2 is null) {
            Console.WriteLine("[×] N_m3u8DL-RE 检测失败，未找到可执行文件");
            return;
        }
        process2.WaitForExit();
        String output2 = process2.StandardOutput.ReadToEnd();
        process2.Dispose();
        if (output.Contains('+')) {
            Console.WriteLine("[√] N_m3u8DL-RE 检测通过");
        } else {
            Console.WriteLine("[×] N_m3u8DL-RE 检测失败，以下是输出详情");
            Console.WriteLine(output2);
        }
    }

    private void CommandURL (ParseResult parseResult) {
        String url = parseResult.GetRequiredValue<String>(this.ArgumentURL);
        Boolean valid = Uri.TryCreate(url, UriKind.Absolute, out Uri? uri);
        if (valid is false || uri is null) {
            Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
            return;
        }
        UInt32 threadCount = parseResult.GetValue<UInt32>(this.OptionThreadCount);
        String type;
        UInt32 vid;
        switch (uri.IdnHost) {
            case "ffzy.tv":
            case "www.ffzy.tv":
            case "ffzy1.tv":
            case "www.ffzy1.tv":
            case "ffzy2.tv":
            case "www.ffzy2.tv":
            case "ffzy3.tv":
            case "www.ffzy3.tv":
            case "ffzy4.tv":
            case "www.ffzy4.tv":
            case "ffzy5.tv":
            case "www.ffzy5.tv":
            case "api.ffzyapi.com":
            case "cj.ffzyapi.com":
            String[] splitedFFZY = uri.AbsolutePath.Replace(".html", String.Empty).Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (splitedFFZY.Length < 1 || String.IsNullOrWhiteSpace(splitedFFZY[^1])) {
                Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
                return;
            }
            if (UInt32.TryParse(splitedFFZY[^1], NumberStyles.Number, CultureInfo.InvariantCulture, out UInt32 resultFFZY) is false || resultFFZY < 1) {
                Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
                return;
            }
            vid = resultFFZY;
            type = "ffzy";
            break;
            case "lzzy.tv":
            case "www.lzzy.tv":
            case "lzizy1.com":
            case "www.lzizy1.com":
            case "lzizy2.com":
            case "www.lzizy2.com":
            case "lzizy3.com":
            case "www.lzizy3.com":
            case "lzizy4.com":
            case "www.lzizy4.com":
            case "lzizy5.com":
            case "www.lzizy5.com":
            case "lzizy6.com":
            case "www.lzizy6.com":
            case "lzizy7.com":
            case "www.lzizy7.com":
            case "lzizy8.com":
            case "www.lzizy8.com":
            String[] splitedLZZY = uri.AbsolutePath.Replace(".html", String.Empty).Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (splitedLZZY.Length < 1 || String.IsNullOrWhiteSpace(splitedLZZY[^1])) {
                Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
                return;
            }
            if (UInt32.TryParse(splitedLZZY[^1], NumberStyles.Number, CultureInfo.InvariantCulture, out UInt32 resultLZZY) is false || resultLZZY < 1) {
                Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
                return;
            }
            vid = resultLZZY;
            type = "lzzy";
            break;
            case "ikunzy.com":
            case "www.ikunzy.com":
            case "ikunzy.net":
            case "www.ikunzy.net":
            case "ikunzy.org":
            case "www.ikunzy.org":
            case "ikunzy.vip":
            case "www.ikunzy.vip":
            case "ikunzyapi.com":
            case "www.ikunzyapi.com":
            String[] splitedIKUNZY = uri.AbsolutePath.Replace(".html", String.Empty).Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (splitedIKUNZY.Length < 1 || String.IsNullOrWhiteSpace(splitedIKUNZY[^1])) {
                Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
                return;
            }
            if (UInt32.TryParse(splitedIKUNZY[^1], NumberStyles.Number, CultureInfo.InvariantCulture, out UInt32 resultIKUNZY) is false || resultIKUNZY < 1) {
                Console.WriteLine("[CommandURL] 资源站链接 {0} 解析失败", url);
                return;
            }
            vid = resultIKUNZY;
            type = "ikunzy";
            break;
            default:
            Console.WriteLine("[CommandURL] 资源站链接 {0} 暂未支持", url);
            return;
        }
        this.DownloadTask(type, vid, threadCount);
    }

    private void CommandVid (ParseResult parseResult) {
        String type = parseResult.GetRequiredValue<String>(this.ArgumentType);
        UInt32 vid = parseResult.GetRequiredValue<UInt32>(this.ArgumentVid);
        UInt32 threadCount = parseResult.GetValue<UInt32>(this.OptionThreadCount);
        this.DownloadTask(type, vid, threadCount);
    }

    private void CommandList (ParseResult parseResult) {
        StringBuilder stringBuilder = new StringBuilder()
            .AppendLine("当前支持的资源站如下：")
            .AppendLine("- [ffzy] / 非凡资源 / api.ffzyapi.com")
            .AppendLine("- [lzzy] / 量子资源 / cj.lziapi.com")
            .AppendLine("- [ikunzy] / IKUN资源 / www.ikunzyapi.com")
            .Append("其它站点陆续支持中");
        Console.WriteLine(stringBuilder.ToString());
    }

    private void DownloadTask (String type, UInt32 vid, UInt32 threadCount) {
        ISiteParserService parserService;
        switch (type) {
            case "ffzy":
            parserService = Program.ServiceProvider.GetRequiredService<FFZYParser.FFZYParserService>();
            break;
            case "lzzy":
            parserService = Program.ServiceProvider.GetRequiredService<LZZYParser.LZZYParserService>();
            break;
            case "ikunzy":
            parserService = Program.ServiceProvider.GetRequiredService<IKUNZYParser.IKUNZYParserService>();
            break;
            default:
            Console.WriteLine("[DownloadTask] 资源站 {0} 暂未支持", type);
            return;
        }
        VodDetailLite? vodDetailLite = parserService.GetVodDetail(vid);
        if (vodDetailLite is null) {
            Console.WriteLine("[DownloadTask] 未取得有效数据，无法启动下载");
            return;
        }
        Console.WriteLine("[DownloadTask] 已取得《{0}》共 {1} 集",vodDetailLite.Name, vodDetailLite.M3U8Queue.Count);
        //System.Diagnostics.Debugger.Break();
        while (vodDetailLite.M3U8Queue.Count > 0) {
            if (vodDetailLite.M3U8Queue.TryDequeue(out String? item) is false) { continue; }
            if (String.IsNullOrWhiteSpace(item)) { continue; }
            String[] splited = item.Split('$', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (splited.Length is not 2) { continue; }
            String saveDir = Path.Combine("__downloads__",String.Concat(Misc.FormatVodName(vodDetailLite.Name),"_",vodDetailLite.Date));
            Process? processDownloader = Process.Start(new ProcessStartInfo() {
                UseShellExecute = false,
                FileName = "N_m3u8DL-RE",
                ArgumentList = {
                    splited[1],
                    "--tmp-dir",
                    "__tmpdir__",
                    "--save-dir",
                    saveDir,
                    "--save-name",
                    splited[0],
                    "--header",
                    Program.UserAgent,
                    "--thread-count",
                    threadCount.ToString()
                },
                // 利用控制台程序无重定向的情况下会直接写父进程的输出来实现立刻中断
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Normal,
                RedirectStandardOutput = false
            });
            if (processDownloader is null) {
                Console.WriteLine("[DownloadTask] 任务《{0}》{1} 启动失败，将再稍后重试", vodDetailLite.Name, splited[0]);
                vodDetailLite.M3U8Queue.Enqueue(item);
                continue;
            }
            processDownloader.WaitForExit();
            Thread.Sleep(1500);
            Console.WriteLine("[DownloadTask] 任务《{0}》{1} 完成", vodDetailLite.Name, splited[0]);
        }
    }
}
