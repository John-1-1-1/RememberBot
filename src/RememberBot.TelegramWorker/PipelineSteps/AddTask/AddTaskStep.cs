using System.Globalization;
using Hors;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramWorker.PipelineSteps.AddTask;

public class AddTaskStep: PipelineStep {
    
    private readonly HorsTextParser _horsTextParser = new();
    
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {

        if (user == null) {
            return new PipelineResult();
        }
        
        var parseTime = _horsTextParser.Parse(message.Text, DateTime.Now);
        
        MessageResult messageResult = AddTaskMessageBuilder.TaskListMessage( 
            parseTime.Dates.Select(d => d.DateTo).ToList(), parseTime.Text, user.TgId);
        
        DataBaseResult dataBaseResult = new DataBaseResult();
        user.AddedText = parseTime.Text;
        dataBaseResult.AddUser(user);
        
        return new PipelineResult {MessageResult = messageResult, DataBaseResult = dataBaseResult};
    }
}