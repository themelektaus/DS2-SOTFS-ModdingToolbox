using System;
using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

using static Utils;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        Icon = null;
    }

    void Button_Backup_Click(object sender, EventArgs e)
    {
        Backup();
    }

    async void Button_DSMapStudio_Click(object sender, EventArgs e)
    {
        await OpenMapStudioAsync();
    }

    async void Button_Build_Click(object sender, EventArgs e)
    {
        await BuildGameAsync(packRegulationFile: false);

        await StartDarkSoulsIIAsync();

        await System.Threading.Tasks.Task.Delay(2000);
        Button_Reload.Enabled = true;

        while (IsProcessRunning("DarkSoulsII"))
            await System.Threading.Tasks.Task.Delay(200);

        Button_Reload.Enabled = false;
        Button_Build.Enabled = true;

        await ClearGameBuildAsync();
    }

    async void Button_Reload_Click(object sender, EventArgs e)
    {
        await AddProjectToGameBuildAsync();
    }
}