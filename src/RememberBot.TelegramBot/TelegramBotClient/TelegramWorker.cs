using System.Security.Cryptography;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramBot.Services;
using RememberBot.TelegramBot.DataBaseContext;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.TelegramBotClient;

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
            
            TelegramUser? user = _dataBaseService.GetUser(update.CallbackQuery?.From.Id ?? update.Message?.Chat.Id);
            
            var pipelineResult = _pipelinesDistributor.Execute(user, 
                new PipelineContext 
                { 
                    Message = update.Message,
                    CallbackQuery = update.CallbackQuery,
                    Type = update.Type 
                });

            if (pipelineResult.Task == MessageTask.GetListTask) {
                var listTasks = _dataBaseService.GetTasksCollection(user.TgId);
                var listTimes = listTasks
                    .GroupBy(t => t.DateTime)
                    .OrderBy(t=> t.First().DateTime);
                
                Client.SendTextMessageAsync(
                    user.TgId, string.Join("\n", listTimes.Select(t => "\ud83d\udccc На "+
                                                                          t.First().DateTime.Add(user.LocalTime) + " \n" + 
                                                                          string.Join("", t.Select( u => "\u2705 " + u.Text + "\n")) ))); 
            }
            
            if (pipelineResult.MessageResult is { TgId: not null, Text: not null }) {
               
                Client.SendTextMessageAsync(pipelineResult.MessageResult.TgId, 
                    pipelineResult.MessageResult.Text,
                    replyMarkup: pipelineResult.MessageResult.ReplyMarkup,
                    cancellationToken: cancellationToken); 
                
                if (pipelineResult.DataBaseResult?.StateUser == DbState.Add) {
                    if (pipelineResult.DataBaseResult.User != null) {
                        _dataBaseService.AddUser(pipelineResult.DataBaseResult.User);
                    }
                }
                
                if (pipelineResult.DataBaseResult?.StateUser == DbState.Update) {
                    if (pipelineResult.DataBaseResult.User != null) {
                        _dataBaseService.UpdateUser(pipelineResult.DataBaseResult.User);
                    }
                }
                
                if (pipelineResult.DataBaseResult?.StateTask == DbState.Add) {
                    if (pipelineResult.DataBaseResult.Task != null) {
                        _dataBaseService.AddTasks(pipelineResult.DataBaseResult.Task);
                    }
                }
                
                if (pipelineResult.DataBaseResult?.StateTask == DbState.Update) {
                    if (pipelineResult.DataBaseResult.Task != null) {
                        _dataBaseService.UpdateTask(pipelineResult.DataBaseResult.Task);
                    }
                }
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