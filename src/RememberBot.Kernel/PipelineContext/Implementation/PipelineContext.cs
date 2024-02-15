using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RememberBot.Kernel.PipelineContext.Implementation;

public class PipelineContext {
    public UpdateType Type;
    public Message? Message;
    public CallbackQuery? CallbackQuery;
}