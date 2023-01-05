using System.Reflection;

namespace TrayStatus.Bootstrapper;

public partial class MainForm : Form
{
    private readonly NotifyIcon icon;

    public MainForm()
    {
        InitializeComponent();

        var closeApplicationMenuItem = new ToolStripMenuItem("Close");
        closeApplicationMenuItem.Click += (_, __) => Application.Exit();

        this.icon = new NotifyIcon()
        {
          Text = "test",
          Visible = true,
          Icon = new Icon("testicon.ico"),
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
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;
        ShowInTaskbar = false;
        base.OnLoad(e);
    }
}
