using System.Reactive.Subjects;
using TrayStatus.Core;
using TrayStatus.StatusProviders.GitLabRunner.Icons;

namespace TrayStatus.StatusProviders.GitLabRunner;

class GitLabRunnerStatusProvider : IStatusProvider
{
    private readonly IconProvider iconProvider;

    public GitLabRunnerStatusProvider(IconProvider iconProvider)
    {
        this.iconProvider = iconProvider ?? throw new ArgumentNullException(nameof(iconProvider));
    }

    public IObservable<Stream> GetIconSource()
    {
        var iconStream = iconProvider.Get();
        return new BehaviorSubject<Stream>(iconStream);
    }

    public IObservable<string> GetTextSource()
    {
        return new BehaviorSubject<string>("Test!");
    }
}