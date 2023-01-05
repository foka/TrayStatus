namespace TrayStatus.Bootstrapper;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        NotifyIcon icon = new NotifyIcon()
        {
          Text = "test",
          Visible = true,
          Icon = new Icon("testicon.ico"),
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        Visible = false;
        ShowInTaskbar = false;
        base.OnLoad(e);
    }
}
