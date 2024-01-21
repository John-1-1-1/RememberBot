using RememberBot.Kernel.PipelineContext.Implementation.Pipeline;
using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = RememberBot.Kernel.PipelineContext.User;

namespace RememberBot.UnitTest;

public class UnitTest1 {
    [Fact]
    public void Test1() {
        Pipeline pipeline = new Pipeline();
        pipeline.AddUnit(new TestPipelineUnit());
        
        var result = pipeline.Execute(new User(), new PipelineContext() {
            Type = UpdateType.Message,
            Message = new Message()
        });

        Assert.Equal("text", result.MessageResult.Text);
        Assert.Equal(123, result.MessageResult.TgId);
    }
}