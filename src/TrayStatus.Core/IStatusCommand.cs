namespace TrayStatus.Core
{
    public class StatusCommand
    {
        public StatusCommand(string text, Action click)
        {
            Text = text;
            Click = click;
        }

        public string Text { get; }
        public Action Click { get; }
    }
}