using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.Kernel.PipelineContext.Implementation.Unit;

public abstract class PipelineStep: APipelineStep {

    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {
        return new PipelineResult();
    }

    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user) {
        return new PipelineResult();
    }

    public override PipelineResult UpdateMessageEmptyUser(Message message) {
        return new PipelineResult();
    }

    public override PipelineResult UpdateCallbackQueryEmptyUser(CallbackQuery callbackQuery) {
        return new PipelineResult();
    }
}