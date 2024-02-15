using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;

namespace RememberBot.TelegramBot.PipelineSteps.None;

public class ListTasksCommand: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {

        if (message.Text == "Список заметок") {
            return new PipelineResult() { Task = new List<MessageTask>() {MessageTask.GetListTask }};
        }

        return new PipelineResult();
    }
}