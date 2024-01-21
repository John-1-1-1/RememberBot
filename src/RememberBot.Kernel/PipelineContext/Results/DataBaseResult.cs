using RememberBot.TelegramWorker.DataBaseContext.Tables;

namespace RememberBot.Kernel.PipelineContext.Results;

public class DataBaseResult {
    public TelegramUser? User;
    public UserTask? Task;
}