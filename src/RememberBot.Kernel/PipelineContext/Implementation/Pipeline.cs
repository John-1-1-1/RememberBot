using RememberBot.Kernel.PipelineContext.Implementation.Unit;
using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.TelegramWorker.DataBaseContext.Tables;
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
            if (pipelineContext.Type == UpdateType.Message && pipelineContext.Message != null) {
                result = unit.UpdateMessage(result, pipelineContext.Message, user);
            }
            if (pipelineContext.Type == UpdateType.CallbackQuery && pipelineContext.CallbackQuery != null) {
                result = unit.UpdateCallbackQuery(result, pipelineContext.CallbackQuery, user);
            }
            if (!result.IsNull()) {
                break;
            }
        }

        return result;
    }
}