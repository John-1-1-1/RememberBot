using RememberBot.Kernel.PipelineContext.Results;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramWorker.PipelineSteps;

public static class StartMessageBuilder {
    
    public static MessageResult SuccessRegistered(long id) => 
        new MessageResult() { 
            ReplyMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[] {
                    new("Список дел"),
                    new("Местное время"),
                }),
            Text = "Вы уже зарегестрированы!",
            TgId = id
        };
    
    public static MessageResult AlreadyRegistered(long id) => 
        new MessageResult() { 
            ReplyMarkup = new ReplyKeyboardMarkup(
            new KeyboardButton[] { 
                new("Список дел"), 
                new("Местное время"),
            }), Text = """
                      Вы успешно зарегистрировались!

                      Для корректной работы бота, воспользуйтесь кнопкой "Часовой пояс", чтобы бот учитывал ваше время при добавлении и отслеживании задач.
                      """,
        TgId = id
};
}