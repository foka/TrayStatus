namespace TrayStatus.Core;

public interface IIconProvider
{
    IObservable<IIcon> IconSource { get; }
}

public interface IIcon
{
    Stream Image { get; }
    string Text { get; }
}
