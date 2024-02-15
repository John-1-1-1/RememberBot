using Telegram.Bot.Types;

namespace RememberBot.Kernel.PipelineContext.Results;

public class PipelineResult {
    public MessageResult MessageResult = new MessageResult();
    public DataBaseResult DataBaseResult = new DataBaseResult();
    public ICollection<MessageTask> Task = new List<MessageTask>();
}

public enum MessageTask {
    GetListTask
}