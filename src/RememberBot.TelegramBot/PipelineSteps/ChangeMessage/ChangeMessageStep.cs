using System.Text.Json;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramBot.PipelineSteps.AddTask;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeMessage;

public class ChangeMessageStep : PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {
        
        var dates = JsonSerializer.Deserialize<List<DateTime>>(user.Times);

        if (dates == null) {
            return new PipelineResult() {MessageResult = 
               new MessageResult() {
                   Text = "Вы не ввели ни одной даты",
                   TgId = user.TgId
               }
            };
        }

        user.AddedText = message.Text ?? user.AddedText;
        user.UserState = TelegramState.AddTask;
        return new PipelineResult() {MessageResult = 
            AddTaskMessageBuilder.TaskListMessage(
                dates, user.AddedText, user.TgId),
            DataBaseResult = new DataBaseResult().UpdateUser(user)
        };
    }
}