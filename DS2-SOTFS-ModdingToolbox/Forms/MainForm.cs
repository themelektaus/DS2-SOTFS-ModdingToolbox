using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

public partial class MainForm : Form
{
    public static MainForm instance { get; private set; }

    readonly BlazorWebView blazorWebView;

    int closeStatus;

    public bool closing => closeStatus > 0;

    public MainForm(Config config)
    {
        SuspendLayout();

        instance = this;

        Text = Lang.Window.TITLE;
        Icon = null;
        FormBorderStyle = FormBorderStyle.None;
        BackColor = Color.Gold;
        TransparencyKey = Color.Gold;
        Location = new(0, 0);
        Size = new(1, 1);
        StartPosition = FormStartPosition.Manual;
        Margin = new(0);

        FormClosing += OnFormClosing;

        blazorWebView = new BlazorWebView
        {
            HostPage = Lang.Internal.INDEX_HTML_PATH,
            Location = new(0, 0),
            Margin = new(0),
            Dock = DockStyle.Fill
        };

        blazorWebView.WebView.DefaultBackgroundColor = Color.Transparent;
        
        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        blazorWebView.Services = services.BuildServiceProvider();
        blazorWebView.RootComponents.Add<UI.App>(
            "#app",
            new Dictionary<string, object>
            {
                { "mainForm", this },
                { "config", config }
            }
        );

        Controls.Add(blazorWebView);

        ResumeLayout(true);
    }

    public void UpdateUI(float scale, bool borderless)
    {
        WindowState = FormWindowState.Normal;

        FormBorderStyle = borderless
            ? FormBorderStyle.None
            : FormBorderStyle.Sizable;

        var z = blazorWebView.WebView.DeviceDpi / 96f;
        Size = new(
            (int) (960 * scale * z),
            (int) (600 * scale * z)
        );

        blazorWebView.WebView.ZoomFactor = scale;

        var s = Screen.PrimaryScreen.Bounds;
        Location = new(
            (s.Width - Size.Width) / 2,
            (s.Height - Size.Height) / 2
        );
    }

    public new void Close()
    {
        if (closeStatus == 0)
            closeStatus = 1;

        base.Close();
    }

    void OnFormClosing(object sender, FormClosingEventArgs e)
    {
        if (closeStatus == 3)
            return;

        e.Cancel = true;

        if (closeStatus == 2)
            return;

        closeStatus = 2;

        Program.taskManager.Stop();

        Task.Run(async () =>
        {
            var now = DateTime.Now;

            while (Program.externalApplicationIsRunning)
                await Utils.WaitShortAsync();

            while (!Program.taskManager.hasStopped)
                await Utils.WaitShortAsync();

            var ms = 250 - (DateTime.Now - now).TotalMilliseconds;
            if (ms > 0)
                await Task.Delay((int) ms);

            closeStatus = 3;

            Invoke(Close);
        });
    }

    [DllImport("user32.dll")]
    static extern void ReleaseCapture();

    [DllImport("user32.dll")]
    static extern void SendMessage(nint hWnd, int Msg, int wParam, int lParam);

    public void Drag()
    {
        ReleaseCapture();
        SendMessage(Handle, 0xA1, 0x2, 0);
    }
}