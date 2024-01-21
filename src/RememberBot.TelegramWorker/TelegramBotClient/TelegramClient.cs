using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.TelegramWorker.TelegramBotClient;

public class TelegramClient {
    public readonly ITelegramBotClient Client;
    private readonly ReceiverOptions _receiverOptions;  
    
    public TelegramClient(ILogger<TelegramClient> logger,
        IServiceProvider serviceProvider, IConfiguration configuration) {
       
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
    
    public void Start() {
        var cts = new CancellationTokenSource();
        Client.StartReceiving(UpdateHandler, ErrorHandler,
            _receiverOptions, cts.Token);
    }
    
    private Task UpdateHandler(ITelegramBotClient botClient,
        Update update, CancellationToken cancellationToken) {
        try { 
            // pipeline
        }
        catch {
            // ignored
        }
        
        return Task.CompletedTask;
    }

    private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }
}