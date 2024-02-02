using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramBot.DataBaseContext;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.UnitTest;

public class TestPipelineStep: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {
        var pipelineResult = new PipelineResult(); 
        pipelineResult.MessageResult.Text = "text";
        pipelineResult.MessageResult.TgId = 123;
        return pipelineResult;
    }
}

public class UnitTest1 {
    [Fact]
    public void PipelineTest_TrueResult() {
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
    public void PipelinesDistributor_TrueResult() {
        PipelinesDistributor pipelinesDistributor = new PipelinesDistributor();
        Pipeline pipeline = new Pipeline();
        pipeline.AddUnit(new TestPipelineStep());
        pipelinesDistributor.AddUnit(pipeline, TelegramState.None);
        
        var result = pipelinesDistributor.Execute(
            new TelegramUser(), new PipelineContext() {
            Type = UpdateType.Message,
            Message = new Message()
        });

        Assert.Equal("text", result.MessageResult.Text);
        Assert.Equal(123, result.MessageResult.TgId);
    }
}