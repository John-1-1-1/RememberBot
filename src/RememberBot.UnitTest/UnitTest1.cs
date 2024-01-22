using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.TelegramWorker.DataBaseContext;
using RememberBot.TelegramWorker.DataBaseContext.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.UnitTest;

public class TestPipelineStep: PipelineStep {
    public override PipelineResult UpdateMessage(PipelineResult pipelineResult, Message message, TelegramUser? user) {
        pipelineResult.MessageResult.Text = "text";
        pipelineResult.MessageResult.TgId = 123;
        return base.UpdateMessage(pipelineResult, message, user);
    }
}

public class UnitTest1 {
    
    [Fact]
    public void Test1() {
        Pipeline pipeline = new Pipeline();
        pipeline.AddUnit(new TestPipelineStep());
        
        var result = pipeline.Execute(new TelegramUser(), new PipelineContext() {
            Type = UpdateType.Message,
            Message = new Message()
        });

        Assert.Equal("text", result.MessageResult.Text);
        Assert.Equal(123, result.MessageResult.TgId);
    }
    
    [Fact]
    public void Test2() {
        PipelinesDistributor pipelinesDistributor = new PipelinesDistributor();
        Pipeline pipeline = new Pipeline();
        pipeline.AddUnit(new TestPipelineStep());
        pipelinesDistributor.AddUnit(pipeline, TelegramState.None);
        
        var result = pipelinesDistributor.Execute(
            new TelegramUser(), new PipelineContext() {
            Type = UpdateType.Message,
            Message = new Message()
        }, TelegramState.None);

        Assert.Equal("text", result.MessageResult.Text);
        Assert.Equal(123, result.MessageResult.TgId);
    }
}