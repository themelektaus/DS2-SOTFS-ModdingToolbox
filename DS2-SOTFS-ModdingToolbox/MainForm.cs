using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

public partial class MainForm : Form
{
    int closeStatus;

    public MainForm()
    {
        InitializeComponent();

        Icon = null;

        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif

        webView.Services = services.BuildServiceProvider();
        webView.RootComponents.Add<Pages.Index>("#app");
    }

    void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (closeStatus == 2)
            return;

        e.Cancel = true;

        if (closeStatus == 1)
            return;

        closeStatus = 1;

        Task.Run(async () =>
        {
            while (Utils.processing)
                await Task.Delay(500);

            closeStatus = 2;
            
            Invoke(Close);
        });
    }
}