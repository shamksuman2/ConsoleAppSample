using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleAppSample.Workflow
{
    public interface IGoodByeWorld
    {
        ExecutionResult Run(IStepExecutionContext context);
    }
}