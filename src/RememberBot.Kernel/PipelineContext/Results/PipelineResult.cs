using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Results;

public class PipelineResult {
    public MessageResult? MessageResult;
    public DataBaseResult? DataBaseResult;
    public CallbackResult? CallbackResult;
    public ICollection<MessageTask> Task = new List<MessageTask>();
}

public enum MessageTask {
    GetListTask
}