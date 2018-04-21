using System;
using System.Collections.Generic;
using System.Text;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace ConsoleAppSample.dynamic
{
    public class PythonIntrop
    {
        public void PythonIntropProgram()
        {
            ScriptEngine engine = Python.CreateEngine();
            string simpleexpression = "2 + 2";
            dynamic dynamicResult = engine.Execute(simpleexpression);

           Console.WriteLine($"Expression Result : {dynamicResult}");
        }

        public void PythonExcutionfromConsole()
        {
            ScriptEngine engine = Python.CreateEngine();
            int customerAge = 24;
            Console.WriteLine("Enter the expression");
            var expression = Console.ReadLine();
            ScriptScope scope = engine.CreateScope();
            scope.SetVariable("a", customerAge);
            ScriptSource source = engine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);

            dynamic dynamicResult = source.Execute(scope);

            Console.WriteLine($"Expression Result : {dynamicResult}");
        }

    }
}
