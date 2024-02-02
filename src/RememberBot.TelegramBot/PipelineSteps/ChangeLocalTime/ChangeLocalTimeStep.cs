using System.Globalization;
using Hors;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeLocalTime;

public class ChangeLocalTimeStep: PipelineStep {
    
    private readonly HorsTextParser _horsTextParser = new();

    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {
        var parseTime = _horsTextParser.Parse(message.Text, DateTime.Now);
            
        List<DateTime> listDates = parseTime.Dates.Select(d => d.DateTo).ToList();

        List<InlineKeyboardButton[]> _buttons =
            new List<InlineKeyboardButton[]>();
        
        foreach (var date in listDates.Take(5)) {
            _buttons.Add(new InlineKeyboardButton[] {
                InlineKeyboardButton.WithCallbackData(date.ToString(CultureInfo.InvariantCulture),
                    "c" + date.ToFileTime())
            });
        }
        
        MessageResult messageResult = new MessageResult();
        
        messageResult.ReplyMarkup = new InlineKeyboardMarkup(_buttons);
        messageResult.Text = "Выберете ваше время!";
        messageResult.TgId = message.Chat.Id;
        
        return new PipelineResult {MessageResult = messageResult};
    }
}