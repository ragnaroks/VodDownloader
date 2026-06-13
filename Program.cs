using System;
using Microsoft.Extensions.DependencyInjection;
using VodDownloader.Services.CommandLine;
using VodDownloader.Services.FFZYParser;
using VodDownloader.Services.IKUNZYParser;
using VodDownloader.Services.LZZYParser;

namespace VodDownloader;

public static class Program {
    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    public static String UserAgent { get; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";

    public static Int32 Main (String[] args) {
        ServiceCollection services = new();
        _ = services.AddSingleton<CommandLineService>();
        _ = services.AddScoped<FFZYParserService>();
        _ = services.AddHttpClient<FFZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://api.ffzyapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        _ = services.AddScoped<LZZYParserService>();
        _ = services.AddHttpClient<LZZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://cj.lziapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        _ = services.AddScoped<IKUNZYParserService>();
        _ = services.AddHttpClient<IKUNZYParserService>(client => {
            client.Timeout = TimeSpan.FromSeconds(8);
            client.BaseAddress = new("https://www.ikunzyapi.com");
            client.DefaultRequestHeaders.UserAgent.Clear();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Program.UserAgent);
        });
        Program.ServiceProvider = services.BuildServiceProvider();

        CommandLineService commandLineService = Program.ServiceProvider.GetRequiredService<CommandLineService>();
        return commandLineService.RootCommand.Parse(args).Invoke();
    }
}
