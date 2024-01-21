using RememberBot.Kernel.PipelineContext.Implementation.Pipeline;
using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types;
using User = RememberBot.Kernel.PipelineContext.User;

namespace RememberBot.UnitTest;

public class TestPipelineUnit: PipelineUnit {
    public override PipelineResult UpdateMessage(PipelineResult pipelineResult, Message message, User? user) {
        pipelineResult.MessageResult.Text = "text";
        pipelineResult.MessageResult.TgId = 123;
        return base.UpdateMessage(pipelineResult, message, user);
    }
}