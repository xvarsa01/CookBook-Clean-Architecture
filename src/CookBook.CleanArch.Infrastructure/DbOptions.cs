namespace CookBook.CleanArch.Infrastructure;

public record DbOptions
{
    public required string DatabaseDirectory { get; init; }
    public required string DatabaseName { get; init; }
    public string DatabaseFilePath => Path.Combine(DatabaseDirectory, DatabaseName!);

    /// <summary>
    /// Deletes database before application startup
    /// </summary>
    public bool RecreateDatabaseEachTime { get; init; } = false;

    /// <summary>
    /// Seeds DemoData from DbContext on database creation.
    /// </summary>
    public bool SeedDemoData { get; init; } = false;
    public bool UseInMemoryDb { get; init; } = false;
}
