using System;
using System.Collections.Generic;
using System.Text;
using WorkflowCore.Interface;
using ConsoleAppSample.Workflow;
using Microsoft.Extensions.DependencyInjection;
using WorkflowCore.Services;


namespace ConsoleAppSample.Workflow
{
    public class WorkflowProgram
    {
        public void WrokflowStart()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<HelloWorldWorkflow>();
            host.Start();

            host.StartWorkflow("HelloWorld", 1, null);

            Console.ReadLine();
            host.Stop();
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            serviceCollection.AddTransient<IGoodByeWorld, GoodByeWorld>();
            serviceCollection.AddTransient<IHelloWorld, HelloWorld>();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            //config logging
            //var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            //loggerFactory.AddDebug();
            return serviceProvider;
        }

    }
}
