using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Implementation.Pipeline;

public abstract class PipelineUnit: APipelineUnit {
    public override PipelineResult UpdateMessage(PipelineResult pipelineResult, Message message, User? user) {
        return pipelineResult;
    }

    public override PipelineResult UpdateCallbackQuery(PipelineResult pipelineResult, CallbackQuery callbackQuery, User? user) {
        return pipelineResult;
    }
}