using System.Reflection;
using TrayStatus.Core;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace TrayStatus.Bootstrapper;

public partial class MainForm : Form
{
    private readonly Icon defaultIcon = new Icon(SystemIcons.Information, 20, 20);
    private readonly Collection<IDisposable> subscriptions = new Collection<IDisposable>();

    public MainForm(IEnumerable<IStatusProvider> statusProviders)
    {
        InitializeComponent();

        foreach (var notifyIcon in statusProviders.Select(CreateIcon))
        {
            this.components!.Add(notifyIcon);
        }
    }

    private NotifyIcon CreateIcon(IStatusProvider provider)
    {
        var icon = new NotifyIcon()
        {
            Icon = defaultIcon,
            Visible = true,
            ContextMenuStrip = CreateMenu(Enumerable.Empty<StatusCommand>()),
        };
        icon.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic)!;
                mi.Invoke(icon, null);
            }
        };

        Subscribe(provider.IconSource, x => icon.Icon = new Icon(x));
        Subscribe(provider.TextSource, x => icon.Text = x);
        Subscribe(provider.CommandsSource, x => icon.ContextMenuStrip = CreateMenu(x));

        return icon;
    }

    private ContextMenuStrip CreateMenu(IEnumerable<StatusCommand> commands)
    {
        var commandsItems = commands.Select(x =>
        {
            var menuItem = new ToolStripMenuItem(x.Text);
            menuItem.Click += (_, _) => x.Click();
            return menuItem;
        });

        var closeMenuItem = new ToolStripMenuItem("Exit");
        closeMenuItem.Click += (_, _) => Close();

        var menu = new ContextMenuStrip();
        menu.Items.AddRange(commandsItems.ToArray());
        menu.Items.Add("-");
        menu.Items.Add(closeMenuItem);
        return menu;
    }

    private void Subscribe<T>(IObservable<T> observable, Action<T> onNext) => subscriptions.Add(observable.Subscribe(onNext));

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;
        ShowInTaskbar = false;
        base.OnLoad(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        foreach (var x in subscriptions)
            x.Dispose();

        base.OnClosed(e);
    }
}
