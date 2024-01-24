using Microsoft.Extensions.Logging.Abstractions;
using RememberBot.TelegramWorker.TelegramBotClient;

namespace RememberBot.TelegramWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private IServiceProvider _serviceProvider;
    
    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration) {
        _serviceProvider = serviceProvider;
        
        new TelegramClient(new NullLogger<TelegramClient>(), _serviceProvider, configuration: configuration).Start();
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}