namespace DS2_SOTFS_ModdingToolbox;

public class EventListener
{
    readonly List<Action> listeners = new();

    public void AddListener(Action listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Action listener)
    {
        listeners.Remove(listener);
    }

    public void Invoke()
    {
        foreach (var listener in listeners)
            listener.Invoke();
    }
}

public class EventListener<T>
{
    readonly List<Action<T>> listeners = new();

    public void AddListener(Action<T> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Action<T> listener)
    {
        listeners.Remove(listener);
    }

    public void Invoke(T value)
    {
        foreach (var listener in listeners)
            listener.Invoke(value);
    }
}