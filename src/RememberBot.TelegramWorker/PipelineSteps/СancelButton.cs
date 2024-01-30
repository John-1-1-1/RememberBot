using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramWorker.PipelineSteps;

public class CancelButton: PipelineStep {
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser? user) {

        if (callbackQuery.Data == "Cancel") {
            if (user != null) {
                user.UserState = TelegramState.None;
            }
        }

        return new PipelineResult() {
            DataBaseResult = new DataBaseResult().AddUser(user)
        };
    }
}