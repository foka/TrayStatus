using Microsoft.Extensions.DependencyInjection;
using TrayStatus.Core;

namespace TrayStatus.StatusProviders.GitLabRunner;

public static class GitLabRunnerStatusProviderServiceCollectionExtensions
{
    public static IServiceCollection AddGitLabRunnerStatus(this IServiceCollection services) => services.AddStatusProvider<GitLabRunnerStatusProvider>();
}
