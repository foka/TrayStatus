namespace TrayStatus.Core;

public interface IStatusProvider
{
    IObservable<Stream> GetIconSource();
    IObservable<string> GetTextSource();
}
