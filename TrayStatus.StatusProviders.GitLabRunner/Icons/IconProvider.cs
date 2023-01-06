namespace TrayStatus.StatusProviders.GitLabRunner.Icons;

class IconProvider
{
    public Stream Get() => GetIcon("grey.ico");

    private Stream GetIcon(string iconFileName) => GetType().Assembly.GetManifestResourceStream($"{GetType().Namespace}.{iconFileName}")!;
}
