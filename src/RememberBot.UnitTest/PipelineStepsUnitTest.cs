using RememberBot.Kernel.Tables;
using RememberBot.TelegramWorker.PipelineSteps;
using RememberBot.TelegramWorker.PipelineSteps.None;
using Telegram.Bot.Types;

namespace RememberBot.UnitTest;

public class PipelineStepsUnitTest {
    [Fact]
    public void StartCommand_NullMessage_EmptyResult() {
        StartCommand startCommand = new StartCommand();
        
        var result = startCommand.UpdateMessage(new Message(), new TelegramUser());
        
        Assert.Null(result.MessageResult);
        Assert.Null(result.DataBaseResult);
    }
    
    [Fact]
    public void StartCommand_NullUser_EmptyResult() {
        StartCommand startCommand = new StartCommand();
        
        var result = startCommand.UpdateMessage(
            new Message{Text = "/start", Chat = new Chat(){Id = 1}}, null);
        
        Assert.NotNull(result.MessageResult);
        Assert.NotNull(result.DataBaseResult);
    }
    
    [Fact]
    public void StartCommand_NotNullUser_EmptyResult() {
        StartCommand startCommand = new StartCommand();
        
        var result = startCommand.UpdateMessage(
            new Message{Text = "/start",Chat = new Chat(){Id = 1}}, new TelegramUser());
        
        Assert.NotNull(result.MessageResult);
        Assert.Null(result.DataBaseResult);
    }
}