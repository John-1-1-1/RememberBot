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
        
        //DateTime.Now.Add(user.LocalTime)
        
        var parseTime = _horsTextParser.Parse(message.Text, DateTime.Now);
            
        List<DateTime> listDates = parseTime.Dates.Select(d => d.DateTo).ToList();

        List<InlineKeyboardButton[]> _buttons =
            new List<InlineKeyboardButton[]>();
        
        foreach (var date in listDates.Take(5)) {
            _buttons.Add(new InlineKeyboardButton[] {
                InlineKeyboardButton.WithCallbackData(date.ToString(CultureInfo.InvariantCulture),
                    "t" + date.ToFileTime())
            });
        }

        _buttons.Add(new InlineKeyboardButton[] {
            InlineKeyboardButton.WithCallbackData("Изменить дату", "ChangeDate"),
            InlineKeyboardButton.WithCallbackData("Изменить текст", "ChangeText")
        });
        
        MessageResult messageResult = new MessageResult();
        
        messageResult.ReplyMarkup = new InlineKeyboardMarkup(_buttons);
        messageResult.Text = parseTime.Text;
        messageResult.TgId = message.Chat.Id;

        DataBaseResult dataBaseResult = new DataBaseResult();

        user.AddedText = messageResult.Text;
        dataBaseResult.AddUser(user);
        
        return new PipelineResult {MessageResult = messageResult, DataBaseResult = dataBaseResult};
    }
}