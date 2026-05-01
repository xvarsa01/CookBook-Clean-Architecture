namespace CookBook.CleanArch.Infrastructure;

public record DbOptions
{
    public required string DatabaseDirectory { get; init; }
    public required string DatabaseName { get; init; }
    public string DatabaseFilePath => Path.Combine(DatabaseDirectory, DatabaseName!);

    public bool RecreateDatabaseEachTime { get; init; } = false;    // Deletes database before application startup
    public bool SeedDemoData { get; init; } = false;
}
