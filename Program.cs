using System;
using Microsoft.Extensions.DependencyInjection;

namespace VodDownloader;

public static class Program {
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public static String UserAgent { get; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";

    public static Int32 Main (String[] args) {
        ServiceCollection services = new();
        _ = services.AddSingleton<Services.CommandLine.CommandLineService>();
        _ = services.AddSingleton<Services.AppSetting.AppSettingService>();
        _ = services.AddScoped<Services.FFZYParser.FFZYParserService>();
        _ = services.AddHttpClient<Services.FFZYParser.FFZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://api.ffzyapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        _ = services.AddScoped<Services.LZZYParser.LZZYParserService>();
        _ = services.AddHttpClient<Services.LZZYParser.LZZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://cj.lziapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        _ = services.AddScoped<Services.IKUNZYParser.IKUNZYParserService>();
        _ = services.AddHttpClient<Services.IKUNZYParser.IKUNZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://www.ikunzyapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        _ = services.AddScoped<Services.YHZYParser.YHZYParserService>();
        _ = services.AddHttpClient<Services.YHZYParser.YHZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://m3u8.apiyhzy.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        _ = services.AddScoped<Services.BFZYParser.BFZYParserService>();
        _ = services.AddHttpClient<Services.BFZYParser.BFZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://bfzyapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        Program.ServiceProvider = services.BuildServiceProvider();

        try {
            _ = Program.ServiceProvider.GetRequiredService<Services.AppSetting.AppSettingService>();
        } catch (Exception ex) {
            Console.WriteLine("解析配置文件时发生异常，{0}",ex.Message);
            return 1;
        }
        
        Services.CommandLine.CommandLineService commandLineService = Program.ServiceProvider.GetRequiredService<Services.CommandLine.CommandLineService>();
        return commandLineService.RootCommand.Parse(args).Invoke();
    }
}
