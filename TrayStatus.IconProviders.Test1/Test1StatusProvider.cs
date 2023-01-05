using TrayStatus.Core;

namespace TrayStatus.IconProviders.Test1;

class Test1StatusProvider : IStatusProvider
{
    public IObservable<Stream> GetIconSource()
    {
        throw new NotImplementedException();
    }

    public IObservable<string> GetTextSource()
    {
        throw new NotImplementedException();
    }
}