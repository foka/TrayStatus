namespace TrayStatus.StatusProviders.GitLabRunner.Icons;

class IconProvider
{
    public Stream GetRunning() => GetIcon("color.ico");
    public Stream GetNotRunning() => GetIcon("grey.ico");

    private Stream GetIcon(string iconFileName) => GetType().Assembly.GetManifestResourceStream($"{GetType().Namespace}.{iconFileName}")!;
}
