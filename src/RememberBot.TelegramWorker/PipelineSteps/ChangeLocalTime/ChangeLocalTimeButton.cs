using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramWorker.PipelineSteps.ChangeLocalTime;

public class ChangeLocalTimeButton : PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {
        if (message.Text == "Главное меню") {
            if (user != null) {
                user.UserState = TelegramState.ChangeLocalTime;
            }
        }

        return new PipelineResult() {
            DataBaseResult = new DataBaseResult().AddUser(user)
        };
    }

    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser? user) {
        if (callbackQuery.Data == "ChangeLocalTime") {
            if (user != null) {
                user.UserState = TelegramState.ChangeLocalTime;
            }
        }

        return new PipelineResult() {
            DataBaseResult = new DataBaseResult().AddUser(user)
        };
    }
}