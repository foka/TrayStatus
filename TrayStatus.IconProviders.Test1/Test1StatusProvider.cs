using System.Reactive.Subjects;
using TrayStatus.Core;

namespace TrayStatus.IconProviders.Test1;

class Test1StatusProvider : IStatusProvider
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