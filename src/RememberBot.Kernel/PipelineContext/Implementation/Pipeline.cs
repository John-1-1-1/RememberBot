using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;
using Telegram.Bot.Types.Enums;

namespace RememberBot.Kernel.PipelineContext.Implementation;

public class Pipeline {
    private readonly ICollection<PipelineStep> _pipelineUnits = new List<PipelineStep>();

    public Pipeline AddUnit(PipelineStep pipelineStep) {
        _pipelineUnits.Add(pipelineStep);
        return this;
    }

    public PipelineResult Execute(TelegramUser? user, PipelineContext pipelineContext) {

        PipelineResult result = new PipelineResult();
        
        foreach (var unit in _pipelineUnits) {
            result = unit.Execute(pipelineContext, user);
            if (result.MessageResult?.TgId != null || result.Task.Count != 0) {
                break;
            }
        }

        return result;
    }
}