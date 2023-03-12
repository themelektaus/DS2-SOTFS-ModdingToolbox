namespace DS2_SOTFS_ModdingToolbox;

public class ResponsiveEventListener<TOut>
{
    readonly List<Func<TOut>> listeners = new();

    public void AddListener(Action listener)
    {
        AddListener(() =>
        {
            listener.Invoke();
            return default;
        });
    }

    public void AddListener(Func<TOut> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Func<TOut> listener)
    {
        listeners.Remove(listener);
    }

    public TOut Invoke()
    {
        var @out = default(TOut);
        foreach (var listener in listeners)
        {
            var result = listener.Invoke();
            if (result is not null)
                @out = result;
        }
        return @out;
    }
}

public class ResponsiveEventListener<T, TOut>
{
    readonly List<Func<T, TOut>> listeners = new();

    public void AddListener(Action<T> listener)
    {
        AddListener(x =>
        {
            listener.Invoke(x);
            return default;
        });
    }

    public void AddListener(Func<T, TOut> listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(Func<T, TOut> listener)
    {
        listeners.Remove(listener);
    }

    public TOut Invoke(T value)
    {
        var @out = default(TOut);
        foreach (var listener in listeners)
        {
            var result = listener.Invoke(value);
            if (result is not null)
                @out = result;
        }
        return @out;
    }
}