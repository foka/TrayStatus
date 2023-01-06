using System.Reactive.Subjects;
using TrayStatus.Core;

namespace TrayStatus.StatusProviders.GitLabRunner;

class GitLabRunnerStatusProvider : IStatusProvider
{
    public IObservable<Stream> GetIconSource()
    {
        var iconStream = GetType().Assembly.GetManifestResourceStream($"{GetType().Namespace}.icon.ico")!;
        return new BehaviorSubject<Stream>(iconStream);
    }

    public IObservable<string> GetTextSource()
    {
        return new BehaviorSubject<string>("Test!");
    }
}