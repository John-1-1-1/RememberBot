using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeMessage;

public class ChangeMessageCallback: PipelineStep {
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user) {

        PipelineResult pipelineResult = new PipelineResult();
        
        if (callbackQuery.Data == "ChangeText") {

            user.UserState = TelegramState.ChangeMessage;
            pipelineResult.DataBaseResult.UpdateUser(user);
            pipelineResult.MessageResult = new MessageResult() {
                Text = "Напишите текст, на который хотите заменить.",
                TgId = user.TgId
                
            };
        }


        return pipelineResult;
    }
}