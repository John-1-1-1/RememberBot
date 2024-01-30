using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramWorker.PipelineSteps;

public class ChangeLocalTimeButton: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {

        PipelineResult pipelineResult = new PipelineResult();
        
        if (user == null) {
            return pipelineResult;
        }

        if (message.Text == "Местное время") {
            user.UserState = TelegramState.ChangeLocalTime;
            pipelineResult.MessageResult = new MessageResult() {
                ReplyMarkup =  new InlineKeyboardMarkup( 
                    new [] {
                        InlineKeyboardButton.
                            WithCallbackData("Отмена",
                                "Cancell"),
                        InlineKeyboardButton.
                            WithCallbackData("Сменить дату",
                                "ChangeData")
                    }
                ),
                Text = "Вы действительно хотите сменить дату?",
                TgId = message.Chat.Id
            };
            pipelineResult.DataBaseResult = new DataBaseResult()
                .AddUser(user);
        }

        return pipelineResult;
    }
}