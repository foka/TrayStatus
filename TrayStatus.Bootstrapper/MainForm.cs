namespace TrayStatus.Bootstrapper;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        var closeApplicationMenuItem = new ToolStripMenuItem("Close");
        closeApplicationMenuItem.Click += (_, __) => Application.Exit();

        NotifyIcon icon = new NotifyIcon()
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
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;
        ShowInTaskbar = false;
        base.OnLoad(e);
    }
}
