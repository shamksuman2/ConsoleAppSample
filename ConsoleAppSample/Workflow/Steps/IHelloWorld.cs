using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleAppSample.Workflow
{
    public interface IHelloWorld
    {
        ExecutionResult Run(IStepExecutionContext context);
    }
}