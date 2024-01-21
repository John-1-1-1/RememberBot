using RememberBot.Kernel.PipelineContext.Implementation.Pipeline;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.TelegramWorker.DataBaseContext.Tables;
using Telegram.Bot.Types;

namespace RememberBot.UnitTest;

public class TestPipelineUnit: PipelineUnit {
    public override PipelineResult UpdateMessage(PipelineResult pipelineResult, Message message, TelegramUser? user) {
        pipelineResult.MessageResult.Text = "text";
        pipelineResult.MessageResult.TgId = 123;
        return base.UpdateMessage(pipelineResult, message, user);
    }
}