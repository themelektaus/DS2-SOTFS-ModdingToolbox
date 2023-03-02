using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

public class TopLevelForm : Form
{
    protected override bool ShowWithoutActivation => true;

    public TopLevelForm() : base()
    {
        TopMost = true;
        TopLevel = true;
    }

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams createParams = base.CreateParams;
            createParams.ExStyle |= 0x00000008;
            return createParams;
        }
    }
}