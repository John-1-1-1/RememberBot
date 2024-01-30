using Telegram.Bot.Types.ReplyMarkups;

namespace RememberBot.Kernel.PipelineContext.Results;

public class MessageResult {
    public IReplyMarkup? ReplyMarkup;
    public string? Text;
    public long? TgId;

    public MessageResult() { }

    public MessageResult(long tgId, string text, ReplyKeyboardMarkup replyMarkup) { 
        ReplyMarkup = replyMarkup; 
        Text = text;
        TgId = tgId;
    }
}