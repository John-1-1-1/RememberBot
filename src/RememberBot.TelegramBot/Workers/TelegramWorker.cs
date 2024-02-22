using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.TelegramBot.Workers;

public class TelegramWorker(ILogger<TelegramWorker> logger, 
    IServiceProvider serviceProvider) : BackgroundService {
    
    private readonly DataBaseService _dataBaseService =
        serviceProvider.GetService<DataBaseService>() ?? 
        throw new Exception("DataBaseService is null");
    private readonly PipelinesDistributor _pipelinesDistributor = 
        serviceProvider.GetService<PipelinesDistributor>() ?? 
        throw new Exception("PipelinesDistributor is null");
    private readonly TelegramBotService _telegramBotService = 
        serviceProvider.GetService<TelegramBotService>() ?? 
        throw new Exception("TelegramBotService is null");

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

            foreach (var messageTask in pipelineResult.Task) {
                if (messageTask == MessageTask.GetListTask) {
                    var listTasks = _dataBaseService.GetTasksCollection(user.TgId);
                    var listTimes = listTasks
                        .GroupBy(t => t.DateTime)
                        .OrderBy(t=> t.First().DateTime);

                    var message = string.Join("\n", 
                        listTimes.Select(t => "\ud83d\udccc На " + 
                                              t.First().DateTime.Add(-user.LocalTime) + " \n" + 
                                              string.Join("", 
                                                  t.Select(u => "\u2705 " + u.Text + "\n"))));
                    _telegramBotService.Client.SendTextMessageAsync(
                        user.TgId, (message == string.Empty?"Задачи ещё не добавлены": message)); 
                }
            }
            
            var callbackQueryId = update.CallbackQuery?.Id;
            if (update.Type == UpdateType.CallbackQuery && callbackQueryId != null) {
                _telegramBotService.Client.AnswerCallbackQueryAsync(callbackQueryId, null);
            }
            
            if (pipelineResult.MessageResult is { TgId: not null, Text: not null }) {
               
                _telegramBotService.Client.SendTextMessageAsync(pipelineResult.MessageResult.TgId, 
                    pipelineResult.MessageResult.Text,
                    replyMarkup: pipelineResult.MessageResult.ReplyMarkup,
                    cancellationToken: cancellationToken);

                if (pipelineResult.DataBaseResult?.User != null) {
                    if (pipelineResult.DataBaseResult?.StateUser == DbState.Add) {
                        _dataBaseService.AddUser(pipelineResult.DataBaseResult.User);
                    }
                    if (pipelineResult.DataBaseResult?.StateUser == DbState.Update) {
                        _dataBaseService.UpdateUser(pipelineResult.DataBaseResult.User);
                    }
                }

                if (pipelineResult.DataBaseResult?.Task != null) {
                    if (pipelineResult.DataBaseResult?.StateTask == DbState.Add) {
                        _dataBaseService.AddTasks(pipelineResult.DataBaseResult.Task);
                    }
                    if (pipelineResult.DataBaseResult?.StateTask == DbState.Update) {
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
        logger.LogInformation("TelegramWorker running at: {time}", DateTimeOffset.Now);
        _telegramBotService.Client.StartReceiving(UpdateHandler, ErrorHandler,
            _telegramBotService.ReceiverOptions, stoppingToken);
        return Task.CompletedTask;
    }
}