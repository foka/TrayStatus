using Microsoft.Extensions.DependencyInjection;

namespace TrayStatus.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStatusProvider<TProvider>(this IServiceCollection services)
        where TProvider : class, IStatusProvider
    {
        return services.AddSingleton<IStatusProvider, TProvider>();
    }
}
