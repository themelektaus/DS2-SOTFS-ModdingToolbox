public class Test
{
    public string text = "Hello, World!";
    public float value = 42;

    public string Greet()
    {
        return text;
    }

    public float Multiply(float factor = 1.642857142857143f)
    {
        return value * factor;
    }

    public async Task LongOne()
    {
        await Task.Delay(3000);
    }
}