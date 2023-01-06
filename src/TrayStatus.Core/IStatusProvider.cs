namespace TrayStatus.Core;

public interface IStatusProvider
{
    IObservable<Stream> IconSource { get; }
    IObservable<string> TextSource { get; }
    IObservable<IEnumerable<StatusCommand>> CommandsSource { get; }
}
