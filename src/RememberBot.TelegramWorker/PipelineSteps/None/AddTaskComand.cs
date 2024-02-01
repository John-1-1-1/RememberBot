using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramWorker.PipelineSteps.None;

public class AddTaskCommand: PipelineStep {
    public override PipelineResult UpdateMessage(Message message, TelegramUser? user) {
        
        PipelineResult pipelineResult = new PipelineResult();
        
        if (user == null) {
            return pipelineResult;
        }

        if (message.Text == "Добавить заметку") {
            MessageResult messageResult = new MessageResult() {
                ReplyMarkup =  new ReplyKeyboardMarkup( 
                    new [] {
                        new KeyboardButton[] { 
                            new("Главное меню"), 
                        }
                    }
                ),
                Text = "Для добавления напоминания просто напишите боту в формате \"в 11 часов Работа\" ",
                TgId = message.Chat.Id
            };

            user.UserState = TelegramState.AddTask;
            DataBaseResult dataBaseResult = new DataBaseResult()
                .UpdateUser(user);
            pipelineResult.MessageResult = messageResult;
            pipelineResult.DataBaseResult = dataBaseResult;
        }

        return pipelineResult;
    }
}