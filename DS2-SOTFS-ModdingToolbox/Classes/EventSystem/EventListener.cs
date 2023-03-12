namespace DS2_SOTFS_ModdingToolbox;

public interface IEventListener
{
    void RemoveAllListeners();
}

public class EventListener : IEventListener
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

    public void RemoveAllListeners()
    {
        listeners.Clear();
    }

    public void Invoke()
    {
        foreach (var listener in listeners)
            listener.Invoke();
    }
}

public class EventListener<T> : IEventListener
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

    public void RemoveAllListeners()
    {
        listeners.Clear();
    }

    public void Invoke(T value)
    {
        foreach (var listener in listeners)
            listener.Invoke(value);
    }
}

public class EventListener<T1, T2> : IEventListener
{
    readonly List<Action<T1, T2>> listeners = new();

    public void AddListener(Action<T1, T2> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Action<T1, T2> listener)
    {
        listeners.Remove(listener);
    }

    public void RemoveAllListeners()
    {
        listeners.Clear();
    }

    public void Invoke(T1 value1, T2 value2)
    {
        foreach (var listener in listeners)
            listener.Invoke(value1, value2);
    }
}