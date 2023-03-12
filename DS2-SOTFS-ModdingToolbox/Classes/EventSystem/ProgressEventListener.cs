namespace DS2_SOTFS_ModdingToolbox;

public class ProgressEventListener : EventListener<string, float?>
{
    public void Invoke(float value, string info)
    {
        Invoke(info, value);
    }

    public void Invoke(string info)
    {
        Invoke(info, null);
    }

    public void Invoke(float value)
    {
        Invoke(null, value);
    }
}