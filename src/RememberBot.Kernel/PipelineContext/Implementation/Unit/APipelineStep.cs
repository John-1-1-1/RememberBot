using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.TelegramWorker.DataBaseContext.Tables;
using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Implementation.Unit;

public abstract class APipelineStep {
    public abstract PipelineResult UpdateMessage(PipelineResult pipelineResult, 
        Message message, TelegramUser? user); 
    public abstract PipelineResult UpdateCallbackQuery(PipelineResult pipelineResult, 
        CallbackQuery callbackQuery, TelegramUser? user);
}