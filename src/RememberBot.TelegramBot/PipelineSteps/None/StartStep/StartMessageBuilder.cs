using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.None.StartStep;

public static class StartMessageBuilder {
    
    public static MessageResult SuccessRegistered(long id) => 
        new MessageResult() { 
            ReplyMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[] {
                    new("Список заметок"),
                    new("Местное время"),
                    new("Добавить заметку")
                }){ResizeKeyboard = true},
            Text = "Вы уже зарегестрированы!",
            TgId = id,
        };
    
    public static MessageResult AlreadyRegistered(long id) => 
        new MessageResult() { 
            ReplyMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[] {
                    new("Список заметок"),
                    new("Местное время"),
                    new("Добавить заметку")
                }){ResizeKeyboard = true},
            Text = $"""
                    Вы успешно зарегистрировались!
                    
                    Для корректной работы бота, воспользуйтесь кнопкой "Часовой пояс", чтобы бот учитывал ваше время при добавлении и отслеживании задач.
                    """,
        TgId = id
};
}