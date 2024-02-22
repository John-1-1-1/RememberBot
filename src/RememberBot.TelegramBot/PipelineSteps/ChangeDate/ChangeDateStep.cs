using System.Globalization;
using System.Text.Json;
using Hors;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using RememberBot.TelegramBot.PipelineSteps.AddTask;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeDate;

public class ChangeDateStep : PipelineStep {
    
    private readonly HorsTextParser _horsTextParser = new();
    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {
        
        var parseTime = _horsTextParser.Parse(message.Text, DateTime.Now);
        
        List<DateTime> listDates = parseTime.Dates.Select(d => d.DateTo).Take(5).ToList();

        List<InlineKeyboardButton[]> _buttons =
            new List<InlineKeyboardButton[]>();
        
        foreach (var date in listDates) {
            _buttons.Add(new InlineKeyboardButton[] {
                InlineKeyboardButton.WithCallbackData(date.ToString(CultureInfo.CurrentCulture),
                    "c" + date.ToFileTime())
            });
        }

        _buttons.Add(new InlineKeyboardButton[] {
            InlineKeyboardButton.WithCallbackData("Изменить дату", "ChangeDate"),
            InlineKeyboardButton.WithCallbackData("Изменить текст", "ChangeText")
        });
        
        user.UserState = TelegramState.AddTask;
        
        MessageResult messageResult = new MessageResult();
        
        messageResult.ReplyMarkup = new InlineKeyboardMarkup(_buttons);
        messageResult.Text = user.AddedText;
        messageResult.TgId = message.Chat.Id;
        user.Times = JsonSerializer.Serialize(listDates);
        
        return new PipelineResult() {
            MessageResult = messageResult,
            DataBaseResult = new DataBaseResult().UpdateUser(user)
        };
    }
}