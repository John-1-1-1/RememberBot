using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.PipelineSteps.None.StartStep;

public class StartCommand: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {

        PipelineResult pipelineResult = new PipelineResult();

        if (message.Text != "/start") {
            return pipelineResult;
        }
            
        if (user != null) {
            pipelineResult.MessageResult = 
                StartMessageBuilder.SuccessRegistered(message.Chat.Id);
        }
        else {
            pipelineResult.MessageResult = 
                StartMessageBuilder.AlreadyRegistered(message.Chat.Id);
            
            pipelineResult.DataBaseResult = new DataBaseResult()
                .AddUser(new TelegramUser() {
                    TgId = message.Chat.Id });
        }

        return pipelineResult;
    }
}