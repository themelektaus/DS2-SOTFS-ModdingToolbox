using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.WinForms;

using System.Drawing;
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

        AutoScaleMode = AutoScaleMode.None;
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

    public void ShowCenter(float scale)
    {
        var zoom = blazorWebView.WebView.DeviceDpi / 96f;

        //Size = new((int) (960 * scale), (int) (600 * scale));
        //blazorWebView.WebView.ZoomFactor = scale * zoom;

        Size = new((int) (960 * scale * zoom), (int) (600 * scale * zoom));
        blazorWebView.WebView.ZoomFactor = scale;

        var (w, h) = GetScreenSize();
        Location = new((w - Size.Width) / 2, (h - Size.Height) / 2);
    }

    (int w, int h) GetScreenSize()
    {
        var screen = Screen.PrimaryScreen.Bounds;
        return (screen.Width, screen.Height);
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

}