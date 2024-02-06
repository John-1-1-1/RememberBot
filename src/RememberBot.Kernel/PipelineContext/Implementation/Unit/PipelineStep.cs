using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.Kernel.PipelineContext.Implementation.Unit;

public abstract class PipelineStep: APipelineStep {

    private bool _isUserEmpty = false;

    // private void Execute(UpdateType type) {
    //     if (type == UpdateType.Message && pipelineContext.Message != null) {
    //         result = unit.UpdateMessage(pipelineContext.Message, user);
    //     }
    //     if (type == UpdateType.CallbackQuery && pipelineContext.CallbackQuery != null) {
    //         result = unit.UpdateCallbackQuery(pipelineContext.CallbackQuery, user);
    //     }
    // }
    //
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {
        return new PipelineResult();
    }

    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser? user) {
        return new PipelineResult();
    }
}