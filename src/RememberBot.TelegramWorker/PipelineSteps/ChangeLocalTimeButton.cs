using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramWorker.PipelineSteps;

public class ChangeLocalTimeButton: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {

        PipelineResult pipelineResult = new PipelineResult();
        
        if (user == null) {
            return pipelineResult;
        }

        user.UserState = TelegramState.ChangeLocalTime;
        pipelineResult.MessageResult = new MessageResult() {
            ReplyKeyboardMarkup = { },
            Text = "",
            TgId = message.Chat.Id
        };
        
        
        return base.UpdateMessage(message, user);
    }
}