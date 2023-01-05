using Microsoft.Extensions.DependencyInjection;
using TrayStatus.Core;

namespace TrayStatus.IconProviders.Test1;

public static class Test1ServiceCollectionExtensions
{
    public static IServiceCollection AddTest1Icon(this IServiceCollection services) => services.AddStatusProvider<Test1StatusProvider>();
}
