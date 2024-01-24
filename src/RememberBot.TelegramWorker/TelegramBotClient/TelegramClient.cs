using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramWorker.DataBaseContext;
using RememberBot.TelegramWorker.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.TelegramWorker.TelegramBotClient;

public class TelegramClient {
    public readonly ITelegramBotClient Client;
    private readonly ReceiverOptions _receiverOptions;
    private PipelinesDistributor? _pipelinesDistributor;
    private DataBaseService? _dataBaseService;
    
    public TelegramClient(ILogger<TelegramClient> logger,
        IServiceProvider serviceProvider, IConfiguration configuration) {

        _pipelinesDistributor = serviceProvider.GetService<PipelinesDistributor>();
        _dataBaseService = serviceProvider.GetService<DataBaseService>();
        
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
            PipelineContext pipelineContext = new PipelineContext() {
                Message = update.Message,
                CallbackQuery = update.CallbackQuery,
                Type = update.Type 
            };
            
            // User? user = _dataBaseService.GetUser(update)
            //
            // _pipelinesDistributor.Execute( pipelineContext, )
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