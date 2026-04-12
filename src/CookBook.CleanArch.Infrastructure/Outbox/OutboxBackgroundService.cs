using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Infrastructure.Outbox;

public sealed class OutboxBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<OutboxBackgroundService> logger)
    : BackgroundService
{
    private const int OutboxProcessorIntervalInSeconds = 5;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting outbox background service");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var outboxProcessor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();
                
                await outboxProcessor.ProcessOutboxMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(OutboxProcessorIntervalInSeconds), stoppingToken);
        }
    }
}
