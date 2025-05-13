using System.Globalization;
using Hors;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeTask;

public class ChangePriority : PipelineStep {
    
        

        public override PipelineResult UpdateMessage(Message message, TelegramUser user) {
            
            List<InlineKeyboardButton[]> _buttons =
                new List<InlineKeyboardButton[]>();
        
           
                _buttons.Add(new InlineKeyboardButton[] {
                    InlineKeyboardButton.WithCallbackData("Высокий","high1"),
                    InlineKeyboardButton.WithCallbackData("Средний","medium1"),
                    InlineKeyboardButton.WithCallbackData("Низкий","low1"),
                    InlineKeyboardButton.WithCallbackData("Без приоритета","none1")
                });
        
            MessageResult messageResult = new MessageResult();
        
            messageResult.ReplyMarkup = new InlineKeyboardMarkup(_buttons);
            messageResult.Text = "Выберете приоритет:";
            messageResult.TgId = message.Chat.Id;

            return new PipelineResult {MessageResult = messageResult};
        }
    }