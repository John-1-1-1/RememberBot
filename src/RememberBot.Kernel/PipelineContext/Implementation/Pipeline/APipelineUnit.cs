using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Implementation.Pipeline;

public abstract class APipelineUnit {
    public abstract PipelineResult UpdateMessage(PipelineResult pipelineResult, 
        Message message, User? user); 
    public abstract PipelineResult UpdateCallbackQuery(PipelineResult pipelineResult, 
        CallbackQuery callbackQuery, User? user);
}