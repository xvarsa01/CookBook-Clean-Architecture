using System.Runtime.CompilerServices;
using CookBook.Clean.Infrastructure;
using CookBook.Clean.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var options = GetDALOptions();
builder.Services.AddUseCasesServices()
                .InstallInfraServices(options);

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

app.Run();

DbOptions GetDALOptions([CallerFilePath] string sourceFilePath = "")
{
    var relativePath = Path.Combine(Path.GetDirectoryName(sourceFilePath)!,"../CookBook.Clean.Infrastructure");
    DbOptions dalOptions = new()
    {
        DatabaseDirectory = Path.GetFullPath(relativePath),
        DatabaseName = "cookbook.db",
    };
    return dalOptions;
}
