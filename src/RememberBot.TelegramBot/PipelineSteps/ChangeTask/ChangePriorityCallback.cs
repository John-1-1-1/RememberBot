using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.TelegramBot.PipelineSteps.ChangeTask;

public class ChangePriorityCallback: PipelineStep {
    public override PipelineResult UpdateCallbackQuery(CallbackQuery callbackQuery, TelegramUser user) {

        PipelineResult pipelineResult = new PipelineResult();
        
        if (callbackQuery.Data == "none") {

            user.LastTask.Priority = TaskPriority.None;
            
            user.UserState = TelegramState.ChangeMessage;
            pipelineResult.DataBaseResult.UpdateUser(user);
            pipelineResult.DataBaseResult.UpdateTask(user.LastTask);
            pipelineResult.MessageResult = new MessageResult() {
                Text = "Приоритет изменён на: Без приоритета",
                TgId = user.TgId,
                ReplyMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[] {
                new ("Изменить приоритет"),
                new("Добавить категорию"),
                new("Главное меню")
            }) { ResizeKeyboard = true }
            };
        }
        
        if (callbackQuery.Data == "low") {

            user.LastTask.Priority = TaskPriority.Low;
            
            user.UserState = TelegramState.ChangeMessage;
            pipelineResult.DataBaseResult.UpdateUser(user);
            pipelineResult.DataBaseResult.UpdateTask(user.LastTask);
            pipelineResult.MessageResult = new MessageResult() {
                Text = "Приоритет изменён на: Низкий",
                TgId = user.TgId,
                ReplyMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[] {
                        new("Изменить приоритет"),
                        new("Добавить категорию"),
                        new("Главное меню")
                    }) { ResizeKeyboard = true }
                
            };
        }
        
        if (callbackQuery.Data == "medium") {

            user.LastTask.Priority = TaskPriority.Medium;
            
            user.UserState = TelegramState.ChangeMessage;
            pipelineResult.DataBaseResult.UpdateUser(user);
            pipelineResult.DataBaseResult.UpdateTask(user.LastTask);
            pipelineResult.MessageResult = new MessageResult() {
                Text = "Приоритет изменён на: Средний",
                TgId = user.TgId,
                ReplyMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[] {
                        new("Изменить приоритет"),
                        new("Добавить категорию"),
                        new("Главное меню")
                    }) { ResizeKeyboard = true }
                
            };
        }
        
        if (callbackQuery.Data == "high") {

            user.LastTask.Priority = TaskPriority.High;
            
            user.UserState = TelegramState.ChangeMessage;
            pipelineResult.DataBaseResult.UpdateUser(user);
            pipelineResult.DataBaseResult.UpdateTask(user.LastTask);
            pipelineResult.MessageResult = new MessageResult() {
                Text = "Приоритет изменён на: Высокий",
                TgId = user.TgId,
                ReplyMarkup = new ReplyKeyboardMarkup(
                    new KeyboardButton[] {
                        new("Изменить приоритет"),
                        new("Добавить категорию"),
                        new("Главное меню")
                    }) { ResizeKeyboard = true }
            };
        }


        return pipelineResult;
    }
}