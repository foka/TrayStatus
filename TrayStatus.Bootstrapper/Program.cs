using Microsoft.Extensions.DependencyInjection;

namespace TrayStatus.Bootstrapper;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();
        ConfigureServices(services);
        using var serviceProvider = services.BuildServiceProvider();
        {
            Application.Run(serviceProvider.GetRequiredService<MainForm>());
        }
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddSingleton<MainForm>();

        services.AddGitLabRunnerStatus();
    }
}