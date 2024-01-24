using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Implementation.Unit;

public abstract class PipelineStep: APipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {
        return new PipelineResult();
    }

    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser? user) {
        return new PipelineResult();
    }
}