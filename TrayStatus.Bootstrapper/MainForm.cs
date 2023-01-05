using System.Reflection;
using TrayStatus.Core;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace TrayStatus.Bootstrapper;

public partial class MainForm : Form
{
    private readonly Collection<IDisposable> subscriptions = new Collection<IDisposable>();

    public MainForm(IEnumerable<IStatusProvider> statusProviders)
    {
        InitializeComponent();

        foreach (var icon in statusProviders.Select(GetIcon))
        {
            this.components!.Add(icon);
        }
    }

    private NotifyIcon GetIcon(IStatusProvider provider)
    {
        var closeApplicationMenuItem = new ToolStripMenuItem("Close");
        closeApplicationMenuItem.Click += (_, __) => Close();

        var icon = new NotifyIcon()
        {
          Visible = true,
          ContextMenuStrip = new ContextMenuStrip()
          {
            Items =
            {
                closeApplicationMenuItem
            }
          }
        };
        icon.MouseClick += (_, args) =>
        {
            if (args.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic)!;
                mi.Invoke(icon, null);
            }
        };

        var i = provider.GetIconSource().Subscribe(x => icon.Icon = new Icon(x));
        var t = provider.GetTextSource().Subscribe(x => icon.Text = x);
        subscriptions.Add(i);
        subscriptions.Add(t);

        return icon;
    }

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
