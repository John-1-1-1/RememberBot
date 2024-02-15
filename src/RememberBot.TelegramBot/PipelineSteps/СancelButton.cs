using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps;

public class CancelButton: PipelineStep {

    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {
        if (message.Text == "Главное меню") {
            user.UserState = TelegramState.None;
            return new PipelineResult() {
                DataBaseResult = new DataBaseResult().UpdateUser(user),
                MessageResult = new MessageResult {
                    TgId = user.TgId,
                    Text = "Вы перешли в главное меню!",
                    ReplyMarkup = new ReplyKeyboardMarkup(
                        new KeyboardButton[] {
                            new("Список заметок"),
                            new("Местное время"),
                            new("Добавить заметку")
                        }) { ResizeKeyboard = true }
                }
            };
        }

        return new PipelineResult();
    }
    
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user) {

        if (callbackQuery.Data == "Cancel") {
            user.UserState = TelegramState.None;
        }

        return new PipelineResult() {
            DataBaseResult = new DataBaseResult().AddUser(user)
        };
    }
}