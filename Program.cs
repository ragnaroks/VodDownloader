using System;
using Microsoft.Extensions.DependencyInjection;
using VodDownloader.Services.CommandLineService;

namespace VodDownloader;

public static class Program {
    public static IServiceProvider ServiceProvider{get;private set;} = null!;
    
    public static Int32 Main (String[] args) {
        ServiceCollection services = new();
        _ = services.AddSingleton<CommandLineService>();        
        Program.ServiceProvider = services.BuildServiceProvider();

        CommandLineService commandLineService = Program.ServiceProvider.GetRequiredService<CommandLineService>();
        return commandLineService.RootCommand.Parse(args).Invoke();
    }
}
