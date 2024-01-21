namespace RememberBot.Kernel.PipelineContext.Results;

public class PipelineResult {
    public MessageResult MessageResult = new MessageResult();
    public DataBaseResult DataBaseResult = new DataBaseResult();
    
    public bool IsNull() {
        return MessageResult.Text == null && MessageResult.TgId == null;
    }
}