using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace ConsoleAppSample.Workflow
{
    public class GoodByeWorld : StepBody, IGoodByeWorld
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            Console.WriteLine("Good bye world");
            return ExecutionResult.Next();
        }
    }


}
