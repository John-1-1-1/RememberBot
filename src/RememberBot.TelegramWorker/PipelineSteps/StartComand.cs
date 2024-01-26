using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramWorker.PipelineSteps;

public class StartCommand: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {

        PipelineResult pipelineResult = new PipelineResult();

            if (message.Text != "/start") {
            return pipelineResult;
        }
        
        if (user != null) {
            pipelineResult.MessageResult = new MessageResult() {
                ReplyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[] {
                        new("Список дел"),
                        new("Местное время"),
                    }),
                Text = "Вы уже зарегестрированы!",
                TgId = message.Chat.Id
            };
        }
        else {
            pipelineResult.MessageResult = new MessageResult() {
                ReplyKeyboardMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[] { 
                        new("Список дел"), 
                        new("Местное время"),
                    }),
                Text = """
                       Вы успешно зарегистрировались!
                       
                       Для корректной работы бота, воспользуйтесь кнопкой "Часовой пояс", чтобы бот учитывал ваше время при добавлении и отслеживании задач.
                       """,
                TgId = message.Chat.Id
            };

            pipelineResult.DataBaseResult.User = new TelegramUser() {
                TgId = message.Chat.Id };
        }

        return pipelineResult;
    }
}