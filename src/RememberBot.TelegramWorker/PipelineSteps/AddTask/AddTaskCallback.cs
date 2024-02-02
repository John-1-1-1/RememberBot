using System.Runtime.InteropServices.JavaScript;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramWorker.PipelineSteps.AddTask;

public class AddTaskCallback : PipelineStep {
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser? user) {

        if (user == null) {
            return new PipelineResult();
        }
        
        if (user.AddedText == String.Empty) {
            MessageResult messageResult = AddTaskMessageBuilder.NotFoundTextMessage(user.TgId);
            return new PipelineResult { MessageResult = messageResult };
        }
        
        if (callbackQuery.Data != null && callbackQuery.Data[0] == 't') {
            
            DataBaseResult dataBaseResult = new DataBaseResult();
            dataBaseResult.AddTask(new TelegramTask() {
                TgId = callbackQuery.From.Id,
                DateTime = DateTime.FromFileTime(long.Parse(callbackQuery.Data.Remove(0, 1))),
                Text = user.AddedText,
            });

            MessageResult messageResult = AddTaskMessageBuilder.TaskAddedMessage(user.TgId);
            return new PipelineResult() { DataBaseResult = dataBaseResult, MessageResult = messageResult};
        }
        return new PipelineResult();
    }
}