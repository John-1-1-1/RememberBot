using Hors;
using Newtonsoft.Json;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RememberBot.TelegramBot.PipelineSteps.AddTask;

public class AddTaskStep: PipelineStep {
    
    private readonly HorsTextParser _horsTextParser = new();
    
    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {
        
        var parseTime = _horsTextParser.Parse(message.Text, DateTime.Now);
        var dates = parseTime.Dates.Take(5).Select(d => d.DateTo).ToList();
        MessageResult messageResult = AddTaskMessageBuilder.TaskListMessage( 
            dates, parseTime.Text, user.TgId);
        
        DataBaseResult dataBaseResult = new DataBaseResult();
        user.AddedText = parseTime.Text;
        user.Times = JsonSerializer.Serialize(dates);
        dataBaseResult.AddUser(user);
        
        return new PipelineResult {MessageResult = messageResult, DataBaseResult = dataBaseResult};
    }
}