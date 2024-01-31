using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramWorker.PipelineSteps.None;

public class ChangeLocalTimeCommand: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {

        PipelineResult pipelineResult = new PipelineResult();
        
        if (user == null) {
            return pipelineResult;
        }

        if (message.Text == "Местное время") {
            pipelineResult.MessageResult = new MessageResult() {
                ReplyMarkup =  new ReplyKeyboardMarkup( 
                    new [] {
                        new KeyboardButton("Главное меню")
                    }
                ),
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