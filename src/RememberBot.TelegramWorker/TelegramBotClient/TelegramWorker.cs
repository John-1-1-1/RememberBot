using System.Security.Cryptography;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramWorker.DataBaseContext;
using RememberBot.TelegramWorker.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.TelegramWorker.TelegramBotClient;

public class TelegramWorker: BackgroundService {
    public readonly ITelegramBotClient Client;
    private readonly ReceiverOptions _receiverOptions;
    private PipelinesDistributor _pipelinesDistributor;
    private DataBaseService _dataBaseService;
    private ILogger<TelegramWorker> _logger;
    
    public TelegramWorker(ILogger<TelegramWorker> logger,
        IServiceProvider serviceProvider, IConfiguration configuration) {

        _logger = logger;
        _pipelinesDistributor = serviceProvider.GetService<PipelinesDistributor>() ?? 
                                throw new Exception("PipelinesDistributor is null");
        _dataBaseService = serviceProvider.GetService<DataBaseService>() ?? 
                           throw new Exception("DataBaseService is null");
        
        var token = configuration.GetValue<String>("TelegramToken");

        if (token == null) {
            logger.LogError("TelegramToken is null");
            throw new ArgumentNullException(token);
        }
        Client = new Telegram.Bot.TelegramBotClient(token);
        _receiverOptions = new ReceiverOptions() {
            AllowedUpdates = new[] {
                UpdateType.Message,
                UpdateType.CallbackQuery 
            }
        };
    }
    
    private Task UpdateHandler(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken) {
        try {
            PipelineContext pipelineContext = new PipelineContext() {
                Message = update.Message,
                CallbackQuery = update.CallbackQuery,
                Type = update.Type 
            };

            TelegramUser? user = _dataBaseService.GetUser(update.CallbackQuery?.From.Id ?? update.Message?.Chat.Id);
            var pipelineResult = _pipelinesDistributor.Execute(user, pipelineContext);
                if (pipelineResult.MessageResult.TgId != null && pipelineResult.MessageResult.Text != null) {
                Client.SendTextMessageAsync(pipelineResult.MessageResult.TgId, pipelineResult.MessageResult.Text,
                    cancellationToken: cancellationToken);
            }
        }
        catch {
            // ignored
        }
        
        return Task.CompletedTask;
    }

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("TelegramWorker running at: {time}", DateTimeOffset.Now);
        Client.StartReceiving(UpdateHandler, ErrorHandler,
            _receiverOptions, stoppingToken);
        return Task.CompletedTask;
    }
}