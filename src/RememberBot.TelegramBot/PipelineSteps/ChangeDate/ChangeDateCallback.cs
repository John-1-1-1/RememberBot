using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeDate;

public class ChangeDateCallback: PipelineStep {
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user) {

        PipelineResult pipelineResult = new PipelineResult();
        
        if (callbackQuery.Data == "ChangeDate") {

            user.UserState = TelegramState.ChangeDate;
            pipelineResult.DataBaseResult.UpdateUser(user);
            pipelineResult.MessageResult = new MessageResult() {
                Text = "Напишите новую дату.",
                TgId = user.TgId
                
            };
        }


        return pipelineResult;
    }
}