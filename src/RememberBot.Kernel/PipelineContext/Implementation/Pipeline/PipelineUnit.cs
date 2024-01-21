using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.TelegramWorker.DataBaseContext.Tables;
using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Implementation.Pipeline;

public abstract class PipelineUnit: APipelineUnit {
    public override PipelineResult UpdateMessage(PipelineResult pipelineResult, Message message, TelegramUser? user) {
        return pipelineResult;
    }

    public override PipelineResult UpdateCallbackQuery(PipelineResult pipelineResult, 
        CallbackQuery callbackQuery, TelegramUser? user) {
        return pipelineResult;
    }
}