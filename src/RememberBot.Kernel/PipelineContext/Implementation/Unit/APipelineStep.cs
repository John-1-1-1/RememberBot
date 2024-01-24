using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Implementation.Unit;

public abstract class APipelineStep {
    public abstract PipelineResult UpdateMessage(Message message, TelegramUser? user); 
    public abstract PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser? user);
}