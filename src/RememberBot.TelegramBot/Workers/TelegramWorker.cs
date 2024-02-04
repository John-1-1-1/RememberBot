using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.Workers;

public class TelegramWorker: BackgroundService {

    private ILogger<TelegramWorker> _logger;
    private DataBaseService _dataBaseService;
    private PipelinesDistributor _pipelinesDistributor;
    private TelegramBotService _telegramBotService;
    
    public TelegramWorker(ILogger<TelegramWorker> logger,
        IServiceProvider serviceProvider) {
        _logger = logger;
        _pipelinesDistributor = serviceProvider.GetService<PipelinesDistributor>() ?? 
                                throw new Exception("PipelinesDistributor is null");

        _dataBaseService = serviceProvider.GetService<DataBaseService>() ?? 
                           throw new Exception("DataBaseService is null");

        _telegramBotService = serviceProvider.GetService<TelegramBotService>() ??
                              throw new Exception("TelegramBotService is null");
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
                
                _telegramBotService.Client.SendTextMessageAsync(
                    user.TgId, string.Join("\n", listTimes.Select(t => "\ud83d\udccc На "+
                                                                          t.First().DateTime.Add(user.LocalTime) + " \n" + 
                                                                          string.Join("", t.Select( u => "\u2705 " + u.Text + "\n")) ))); 
            }
            
            if (pipelineResult.MessageResult is { TgId: not null, Text: not null }) {
               
                _telegramBotService.Client.SendTextMessageAsync(pipelineResult.MessageResult.TgId, 
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
        _telegramBotService.Client.StartReceiving(UpdateHandler, ErrorHandler,
            _telegramBotService.ReceiverOptions, stoppingToken);
        return Task.CompletedTask;
    }
}