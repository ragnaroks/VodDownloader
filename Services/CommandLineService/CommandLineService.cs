using System;
using System.CommandLine;
using System.Diagnostics;
using System.Text;

namespace VodDownloader.Services.CommandLineService;

public sealed class CommandLineService {
    public RootCommand RootCommand { get; private set; }

    public CommandLineService () {
        Option<Boolean> optionDev = new("--dev") {
            Description = "是否启用开发者模式"
        };

        Command subcommandTest = new("test");
        subcommandTest.SetAction(this.CommandTest);

        Command subcommandURL = new("url");
        subcommandURL.SetAction(this.CommandURL);
        Argument<String> argumentURL = new("URL") {
            Description = "资源库视频详情页面地址",
            Validators = {
                (result) => {
                    String value = result.GetValueOrDefault<String>();
                    Boolean valid = Uri.TryCreate(value,UriKind.Absolute,out Uri? newValue);
                    if (valid is true && newValue is not null) {return;}
                    result.AddError("参数 <URL> 错误，反序列化失败");
                }
            }
        };
        subcommandURL.Arguments.Add(argumentURL);
        
        Command subcommandVid = new("vid");
        subcommandVid.SetAction(this.CommandVid);
        Argument<String> argumentType = new("TYPE") {
            Description = "资源库类型，例如 ffzy、lzzy 等 ，详情请执行 list 子命令查看"
        };
        Argument<UInt32> argumentVid = new("VID") {
            Description = "视频 id，例如 114514 等",
            /*Validators = {
                (result) => {
                    UInt32 value = result.GetValueOrDefault<UInt32>();
                    Boolean b1 = UInt32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out UInt32 newValue);
                    if (b1 is true && newValue is not null) {return;}
                    result.AddError("选项 --vid 参数错误");
                }
            }*/
        };
        subcommandVid.Arguments.Add(argumentType);
        subcommandVid.Arguments.Add(argumentVid);

        Command subcommandList = new("list");
        subcommandList.SetAction(this.CommandList);

        this.RootCommand = new("VodDownloader");
        this.RootCommand.Options.Add(optionDev);
        this.RootCommand.Subcommands.Add(subcommandTest);
        this.RootCommand.Subcommands.Add(subcommandURL);
        this.RootCommand.Subcommands.Add(subcommandVid);
        this.RootCommand.Subcommands.Add(subcommandList);
    }

    private void CommandTest(ParseResult parseResult) {
        Process? process = null;
        try {
            process = Process.Start(new ProcessStartInfo(){
                UseShellExecute = false,
                FileName = "ffmpeg",
                Arguments = "-version",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            });
        }catch(Exception ex) {
            process?.Dispose();
            Console.WriteLine("[×] ffmpeg 检测失败，以下是输出详情");
            Console.WriteLine(ex.Message);
            return;
        }
        if(process is null) {
            Console.WriteLine("[×] ffmpeg 检测失败，未找到可执行文件");
            return;
        }
        process.WaitForExit();
        String output = process.StandardOutput.ReadToEnd();
        process.Dispose();        
        if(output.Contains("ffmpeg version") && output.Contains("exit code 0")) {
            Console.WriteLine("[√] ffmpeg 检测通过");
        } else {
            Console.WriteLine("[×] ffmpeg 检测失败，以下是输出详情");
            Console.WriteLine(output);
        }

        Process? process2 = null;
        try {
            process2 = Process.Start(new ProcessStartInfo(){
                UseShellExecute = false,
                FileName = "N_m3u8DL-RE",
                Arguments = "--version",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8
            });
        }catch(Exception ex) {
            process2?.Dispose();
            Console.WriteLine("[×] N_m3u8DL-RE 检测失败，以下是输出详情");
            Console.WriteLine(ex.Message);
            return;
        }
        if(process2 is null) {
            Console.WriteLine("[×] N_m3u8DL-RE 检测失败，未找到可执行文件");
            return;
        }
        process2.WaitForExit();
        String output2 = process2.StandardOutput.ReadToEnd();
        process2.Dispose();        
        if(output.Contains('+')) {
            Console.WriteLine("[√] N_m3u8DL-RE 检测通过");
        } else {
            Console.WriteLine("[×] N_m3u8DL-RE 检测失败，以下是输出详情");
            Console.WriteLine(output2);
        }
    }

    private void CommandURL (ParseResult parseResult) {
        
    }

    private void CommandVid (ParseResult parseResult) {

    }

    private void CommandList (ParseResult parseResult) {
        StringBuilder stringBuilder = new StringBuilder()
            .AppendLine("当前支持的资源站如下：")
            .AppendLine("- [ffzy] / 非凡资源站 / api.ffzyapi.com")
            .Append("其它站点陆续支持中");
        Console.WriteLine(stringBuilder.ToString());
    }
}
