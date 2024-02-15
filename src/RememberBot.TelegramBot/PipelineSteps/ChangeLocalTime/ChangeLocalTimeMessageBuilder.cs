using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeLocalTime;

public static class ChangeLocalTimeMessageBuilder {

    public static MessageResult ShowAddedTime(TelegramUser user) =>
        new MessageResult {
            Text = "Ваше время: " + (DateTime.Now - user.LocalTime),
            TgId = user.TgId,
            ReplyMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[] {
                    new("Список заметок"),
                    new("Местное время"),
                    new("Добавить заметку")
                }) { ResizeKeyboard = true }
        };
}