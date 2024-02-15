using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeLocalTime;

public class ChangeLocalTimeCallback : PipelineStep {
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user) {
        if (callbackQuery.Data != null && callbackQuery.Data[0] == 'c') {
            string message = callbackQuery.Data.Remove(0, 1);
            user.UserState = TelegramState.None; 
            user.LocalTime =  DateTime.UtcNow - DateTime.FromFileTime(long.Parse(message)).ToUniversalTime();
            return new PipelineResult() {
                DataBaseResult = new DataBaseResult().AddUser(user),
                MessageResult = ChangeLocalTimeMessageBuilder.ShowAddedTime(user)
            };
        }
        return new PipelineResult();
    }
}