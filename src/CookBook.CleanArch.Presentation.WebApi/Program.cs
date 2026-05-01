using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using CookBook.CleanArch.Application;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Presentation.WebApi.Converters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        JsonOptionsSetup.Configure(options.JsonSerializerOptions);
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.UseInlineDefinitionsForEnums();
});

var options = GetDALOptions(builder.Configuration);
builder.Services.AddApplicationServices()
                .AddInfraServices(options);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.InitializeDatabase();

app.Run();
return;

DbOptions GetDALOptions(IConfiguration configuration, [CallerFilePath] string sourceFilePath = "")
{
    var dbSection = configuration.GetSection("CookBook:DB");
    
    var relativePath = Path.Combine(Path.GetDirectoryName(sourceFilePath)!,"../CookBook.CleanArch.Infrastructure");
    DbOptions dalOptions = new()
    {
        DatabaseDirectory = Path.GetFullPath(relativePath),
        DatabaseName = dbSection["DatabaseName"] ?? "cookbook.db",
        SeedDemoData = TryParseBool(dbSection["SeedDemoData"]),
        RecreateDatabaseEachTime = TryParseBool(dbSection["RecreateDatabaseEachTime"])
    };
    return dalOptions;
}

bool TryParseBool(string? value) => bool.TryParse(value, out var parsed) && parsed;


public static class AppInitialization
{
    public static void InitializeDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var options = scope.ServiceProvider.GetRequiredService<DbOptions>();
        var migrator = scope.ServiceProvider.GetRequiredService<IDbMigrator>();
        migrator.Migrate();

        if (options.SeedDemoData)
        {
            var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
            seeder.Seed();
        }
    }
}

public static class JsonOptionsSetup
{
    public static void Configure(JsonSerializerOptions options)
    {
        options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
        options.Converters.Add(new ValueObjectJsonConverterFactory());
        options.Converters.Add(new JsonStringEnumConverter());
    }
}
