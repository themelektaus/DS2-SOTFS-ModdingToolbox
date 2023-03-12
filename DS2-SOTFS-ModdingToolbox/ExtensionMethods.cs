using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using System.Reflection;

using Flags = System.Reflection.BindingFlags;

namespace DS2_SOTFS_ModdingToolbox;

public static class ExtensionMethods
{
    const Flags PRIVATE_FLAGS = Flags.Instance | Flags.NonPublic;

    public static T Get<T>(this List<T> @this, int index)
    {
        if (index < 0)
            return default;

        if (index >= @this.Count)
            return default;

        return @this[index];
    }

    public static async Task LogAsync(this IJSRuntime @this, params object[] values)
    {
        await @this.InvokeVoidAsync("console.log", values);
    }

    public static void Render(this ComponentBase @this)
    {
        @this.GetPrivateMethod("StateHasChanged").Invoke(@this, null);
    }

    public static Task RenderAsync(this ComponentBase @this)
    {
        var method = @this.GetPrivateMethod("InvokeAsync", typeof(Action));
        var task = method.Invoke(@this, new object[] { new Action(@this.Render) }) as Task;
        return task;
    }

    public static MarkupString ToMarkupString(this string @this)
    {
        return new(@this);
    }

    public static string Format(this string @this, params object[] values)
    {
        return string.Format(@this, values);
    }

    public static float GetUIOpacity(this TaskInstance @this)
    {
        var age = @this.initTimestamp.GetAgeInSeconds();
        var end = @this.endTimestamp;

        float opacity;
        if (age > .2f && end.HasValue)
            opacity = 1 - Math.Max(0, (end?.GetAgeInSeconds() ?? 0) - 3) / .2f;
        else
            opacity = Math.Min(1, age / .2f);

        return MathF.Round(opacity, 2);
    }

    public static float GetAgeInSeconds(this DateTimeOffset @this)
    {
        return (float) (DateTimeOffset.Now - @this).TotalSeconds;
    }

    public static FieldInfo[] GetPrivateFields(this object @object)
    {
        return @object.AsType().GetFields(PRIVATE_FLAGS);
    }

    public static void SetFieldValue(this object @object, string name, object value)
    {
        @object.AsType().GetField(name, PRIVATE_FLAGS).SetValue(@object, value);
    }

    public static MethodInfo GetPrivateMethod(this object @this, string name, params Type[] argTypes)
    {
        return @this.AsType().GetMethod(name, PRIVATE_FLAGS, argTypes);
    }

    static Type AsType(this object @object)
    {
        return @object is Type type ? type : @object.GetType();
    }
}