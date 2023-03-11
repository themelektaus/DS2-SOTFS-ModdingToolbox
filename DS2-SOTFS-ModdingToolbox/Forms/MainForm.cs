using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

using System.Drawing;
using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

public partial class MainForm : Form
{
    public static MainForm instance { get; private set; }

    int closeStatus;

    public bool closing => closeStatus > 0;

    public MainForm()
    {
        instance = this;

        var screen = Screen.PrimaryScreen.WorkingArea;

        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new(96, 96);
        DoubleBuffered = true;
        Text = Lang.Window.TITLE;
        Icon = null;
        BackColor = Color.Black;
        ClientSize = new((int) (screen.Width * .5f), (int) (screen.Height * .5f));
        Margin = new(0);
        StartPosition = FormStartPosition.CenterScreen;

        FormClosing += OnFormClosing;

        var webView = new BlazorWebView
        {
            HostPage = Lang.Internal.INDEX_HTML_PATH,
            Location = new(0, 0),
            Margin = new(0),
            BackColor = Color.Black,
            ForeColor = Color.White,
            Dock = DockStyle.Fill
        };

        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        webView.Services = services.BuildServiceProvider();
        webView.RootComponents.Add<View.App>(
            "#app",
            new Dictionary<string, object>
            {
                { "mainForm", this }
            }
        );

        Controls.Add(webView);
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
            while (Program.externalApplicationIsRunning)
                await Utils.WaitShortAsync();

            while (!Program.taskManager.hasStopped)
                await Utils.WaitShortAsync();

            closeStatus = 3;

            Invoke(Close);
        });
    }

}