using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.None;

public class ChangeLocalTimeCommand: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser user) {

        PipelineResult pipelineResult = new PipelineResult();

        if (message.Text == "Местное время") {
            pipelineResult.MessageResult = new MessageResult() {
                ReplyMarkup =  new ReplyKeyboardMarkup( 
                    new [] {
                        new KeyboardButton("Главное меню")
                    }
                ){ ResizeKeyboard = true },
                Text = "Введите ваше время!",
                TgId = message.Chat.Id
            };
            user.UserState = TelegramState.ChangeLocalTime;
            pipelineResult.DataBaseResult = new DataBaseResult()
                .AddUser(user);
        }

        return pipelineResult;
    }
}