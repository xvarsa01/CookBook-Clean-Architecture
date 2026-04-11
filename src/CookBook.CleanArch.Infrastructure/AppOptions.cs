namespace CookBook.CleanArch.Infrastructure;

public record AppOptions
{
    public required string AppName { get; init; }
    
    public string AppDataDirectory => Path.Combine(SystemAppDataDirectory, AppName);
    private static readonly string SystemAppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
}
