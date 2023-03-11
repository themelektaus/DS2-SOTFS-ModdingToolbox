using System.IO;
using System.Text;

namespace DS2_SOTFS_ModdingToolbox;

public class Logger : IDisposable
{
	public List<LogEntry> entries { get; private set; } = new();

	readonly LogLevel minLowLevel;
	readonly LogLevel minHighLevel;

	readonly MemoryStream stream = new();

	public Logger(LogLevel minLowLevel, LogLevel minHighLevel)
	{
		this.minLowLevel = minLowLevel;
		this.minHighLevel = minHighLevel;
	}

	public void Dispose()
	{
		stream.Dispose();
	}

	public void Log(string sender, string title, Exception ex)
	{
		Log(sender, LogType.Error, title, ex.Message);
	}

    public void Log(LogType type, string title, string text)
    {
        Log(nameof(Logger), LogLevel.Release, type, title, text);
    }

    public void Log(string sender, LogType type, string title, string text)
	{
		Log(sender, LogLevel.Release, type, title, text);
	}

	public void Log(string sender, LogLevel level, LogType type, string title, string text)
	{
		if (level < minLowLevel)
			return;

		var entry = new LogEntry(DateTimeOffset.Now, sender, type, title, text);

		var bytes = Encoding.UTF8.GetBytes(
			$"{entry.timestamp:yyyy-MM-dd HH:mm:ss}\t{entry.sender}\t{entry.type}\t{entry.title}\t{entry.text}{Environment.NewLine}"
		);
		stream.Write(bytes);

		if (level < minHighLevel)
			return;

		entries.Add(entry);
	}

	public void Save()
	{
		var path = Path(UserData.backupFolder, "test.log");
		using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
		this.stream.Position = 0;
		this.stream.CopyTo(stream);
		stream.Close();
	}
}