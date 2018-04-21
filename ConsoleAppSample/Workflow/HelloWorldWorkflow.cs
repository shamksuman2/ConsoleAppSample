using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;

namespace ConsoleAppSample.Workflow
{
    public class HelloWorldWorkflow : IWorkflow
    {
        public string Id => "Hello world";

        public int Version => 1;

        public void Build(IWorkflowBuilder<object> builder)
        {
            builder.StartWith<HelloWorld>().Then<GoodByeWorld>();
        }
    }


    

    

}
