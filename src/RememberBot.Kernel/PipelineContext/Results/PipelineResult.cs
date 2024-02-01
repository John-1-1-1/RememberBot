namespace RememberBot.Kernel.PipelineContext.Results;

public class PipelineResult {
    public MessageResult? MessageResult;
    public DataBaseResult? DataBaseResult;
    public MessageTask Task = MessageTask.None;
}

public enum MessageTask {
    None,
    GetListTask
}