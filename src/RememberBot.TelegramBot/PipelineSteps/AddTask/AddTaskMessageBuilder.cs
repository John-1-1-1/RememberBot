using System.Globalization;
using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.AddTask;

public static class AddTaskMessageBuilder {
    public static MessageResult TaskAddedMessage (long id) => 
        new() {
            Text = "Задача добавлена!",
            TgId = id
        };

    public static MessageResult NotFoundTextMessage(long id) =>
        new() {
            Text = "Текст задачи не найден!",
            TgId = id
        };

    public static MessageResult TaskListMessage(List<DateTime> listDates, string message, long id) {
        
        var buttons = listDates.Take(5).Select(date =>
            new List<InlineKeyboardButton> {InlineKeyboardButton.WithCallbackData(date.ToString(CultureInfo.InvariantCulture),
            "t" + date.ToFileTime())} ).ToList();

        buttons.Add(new List<InlineKeyboardButton> {
            InlineKeyboardButton.WithCallbackData("Изменить дату", "ChangeDate"),
            InlineKeyboardButton.WithCallbackData("Изменить текст", "ChangeText")
        });

        return new MessageResult {
            ReplyMarkup = new InlineKeyboardMarkup(buttons),
            Text = message,
            TgId = id
        };
    }
}