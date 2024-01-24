using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.Kernel.PipelineContext.Results;

public class MessageResult {
    public ReplyKeyboardMarkup? ReplyKeyboardMarkup;
    public string? Text;
    public long? TgId;

    public MessageResult() { }

    public MessageResult(long tgId, string text, ReplyKeyboardMarkup replyKeyboardMarkup) { 
        ReplyKeyboardMarkup = replyKeyboardMarkup; 
        Text = text;
        TgId = tgId;
    }
}