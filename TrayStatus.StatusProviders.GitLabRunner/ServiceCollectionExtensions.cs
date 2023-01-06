using TrayStatus.Core;
using TrayStatus.StatusProviders.GitLabRunner;
using TrayStatus.StatusProviders.GitLabRunner.Icons;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitLabRunnerStatus(this IServiceCollection services)
    {
        services.AddStatusProvider<GitLabRunnerStatusProvider>();

        services.AddSingleton<IconProvider>();

        return services;
    }
}
