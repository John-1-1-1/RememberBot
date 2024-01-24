using RememberBot.Kernel.PipelineContext.Results;
using RememberBot.Kernel.Tables;

namespace RememberBot.Kernel.PipelineContext.Implementation;


public class PipelinesDistributor {

    public readonly List<(TelegramState state, Pipeline pipeline)> Pipelines =
        new List<(TelegramState state, Pipeline pipeline)>();
    
    public PipelinesDistributor AddUnit(Pipeline pipeline, TelegramState state) {
        Pipelines.Add((state, pipeline));
        return this;
    }

    public PipelineResult Execute(TelegramUser? user, PipelineContext pipelineContext, TelegramState state) {
        foreach (var pipeline in Pipelines) {
            if (state == pipeline.state) {
                return pipeline.pipeline.Execute(user, pipelineContext);
            } 
        }

        return new PipelineResult();
    }
}