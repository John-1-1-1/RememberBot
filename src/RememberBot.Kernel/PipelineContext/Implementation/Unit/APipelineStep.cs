using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.Kernel.PipelineContext.Implementation.Unit;

public abstract class APipelineStep {
    
    public PipelineResult Execute(PipelineContext pipelineContext, TelegramUser? user) {

        if (pipelineContext.Type == UpdateType.Message && pipelineContext.Message != null) {
            if (user == null) {
                return UpdateMessageEmptyUser(pipelineContext.Message);
            }

            return UpdateMessage(pipelineContext.Message, user);
        }

        if (pipelineContext.Type == UpdateType.CallbackQuery && pipelineContext.CallbackQuery != null) {
            if (user == null) {
                return UpdateCallbackQueryEmptyUser(pipelineContext.CallbackQuery);
            }
            return UpdateCallbackQuery(pipelineContext.CallbackQuery, user);
        }

        return new PipelineResult();
    }
    
    
    public abstract PipelineResult UpdateMessage(Message message, TelegramUser user); 
    public abstract PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user);
    
    public abstract PipelineResult UpdateMessageEmptyUser(Message message); 
    public abstract PipelineResult UpdateCallbackQueryEmptyUser(CallbackQuery callbackQuery);

}