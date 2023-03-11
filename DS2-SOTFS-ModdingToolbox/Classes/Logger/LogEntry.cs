namespace DS2_SOTFS_ModdingToolbox;

public record LogEntry(
    DateTimeOffset timestamp,
    string sender,
    LogType type,
    string title,
    string text
);