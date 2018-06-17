//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleAppSample.BuilderPattern;
using ConsoleAppSample.CircuitBreaker;
using ConsoleAppSample.dynamic;
using ConsoleAppSample.DesignStrategies;
using ConsoleAppSample.Generic;
//using WorkflowCore.Interface;
using ConsoleAppSample.Workflow;
using Newtonsoft.Json;
//using WorkflowCore.Services;
using ConsoleAppSample.TPL;
using ConsoleAppSample.PubSubPattern;

namespace ConsoleAppSample
{
    public class Program
    {
        const string CustomerJson = "{'FirstName':'Sash', 'LastName': 'kumar'}";

        public static void Main(string[] args)
        {

            NewKeyword n = new NewKeyword();
            n.MainProgram();
            //GenericProgram prog = new GenericProgram();
            //prog.Main();
            ////BuilderPatternMain prog = new BuilderPatternMain();
            ////prog.Main();
            ////CircuitBreakerProgram prog= new CircuitBreakerProgram();
            ////prog.Main();

            ////PubSubProg prog = new PubSubProg();
            ////prog.Main();

            ////TPLDemo tpl = new TPLDemo();
            ////tpl.TaskCompletionSource();
            ////Singleton s1 = Singleton.Instance;
            ////s1.DoSomething("S1");
            ////Singleton s2 = Singleton.Instance;
            ////s1.DoSomething("S2");

            ////PythonIntrop pi = new PythonIntrop();
            ////pi.PythonExcutionfromConsole();
            ////CreateExcel excel = new CreateExcel();
            ////excel.CreateExcelFileFromJson();

            ////dynamic cust = JsonConvert.DeserializeObject(CustomerJson);
            ////Console.WriteLine($"Customer: {cust.FirstName} {cust.LastName}");

            ///*Workflow */
            ////var workflow = new WorkflowProgram();
            ////workflow.WrokflowStart();

            ////CreateExcel excel = new CreateExcel();
            ////excel.CreateExcelFile();

            //Console.WriteLine("Press to exit...");
            //Console.ReadLine();





            //for (int i=0, j = 0; i <= 3 && j <= i ;i++,j++)
            //{
            //    Console.Write("*");
            //    if(i==j) Console.WriteLine();
            //}

            Console.ReadLine();

        }


    }


}