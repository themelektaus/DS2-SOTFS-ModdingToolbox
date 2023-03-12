using System.Windows.Forms;

namespace DS2_SOTFS_ModdingToolbox;

public class Message
{
    public string title;
    public string text;
    public MessageBoxIcon icon;
    public MessageBoxButtons buttons;

    public static bool wasShown { get; private set; }

    Message() { }

    public static Message CreateInfo(string text) => new()
    {
        title = nameof(MessageBoxIcon.Information),
        text = text,
        icon = MessageBoxIcon.Information,
        buttons = MessageBoxButtons.OK
    };

    public static Message CreateWarning(string text) => new()
    {
        title = nameof(MessageBoxIcon.Warning),
        text = text,
        icon = MessageBoxIcon.Warning,
        buttons = MessageBoxButtons.OK
    };

    public static Message CreateError(string text) => new()
    {
        title = nameof(MessageBoxIcon.Error),
        text = text,
        icon = MessageBoxIcon.Error,
        buttons = MessageBoxButtons.OK
    };

    public DialogResult Show()
    {
        return ShowAsync().Result;
    }

    public Task<DialogResult> ShowAsync()
    {
        wasShown = true;
        return Task.Run(() =>
        {
            using var form = new TopLevelForm();
            return MessageBox.Show(form, text, title, buttons, icon);
        });
    }
}