using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppSample.TPL
{
    public class TPLDemo
    {

        public void TaskCompletionSource()
        {
            string logPath = @"C:\Users\h195854\Desktop\Miscellanious\Assignments\ConsoleAppSample\ConsoleAppSample\TPL\web.log";

            
            var processor = new LogProcessor(logPath);
            Thread.Sleep(5000);
        }
        public void Multithreading()
        {

            var webClient = new WebClient();
            Task<string> taskResult = Task<string>.Factory.StartNew(() =>
            {
                string result = "Hello World"; //;webClient.DownloadString("http://localhost:59165/");
                Thread.Sleep(5000);
                return result;
            });

            Console.WriteLine("Setting up Continuation.");

            taskResult.ContinueWith(t =>
            {
                Console.WriteLine($"Web client result{t.Result}");

            });
            Thread.Sleep(10000);
            Console.WriteLine("Continuing on main thread.");
        }
        public void AsyncProgram()
        {
            var webClient1 = new WebClient();
            var webClient2 = new WebClient();
            Task<string> taskResult1 = webClient1.DownloadStringTaskAsync(new Uri("http://localhost:60573/api/values"));
            Task<string> taskResult2 = webClient2.DownloadStringTaskAsync(new Uri("http://localhost:60573/api/valuess"));

            Task<string[]> taskResult = Task.WhenAll(taskResult1, taskResult2);

            Console.WriteLine("Setting up Continuation.");
            Task.Factory.ContinueWhenAll(new[] {taskResult1, taskResult2}, (Task<string>[] tasks) =>
            {
                foreach (var t in tasks)
                {
                    if (t.IsFaulted)
                        Console.WriteLine($"Web client result{t.Exception}");
                    else
                        Console.WriteLine($"Web client result{t.Result}");
                }
            }).Wait();

            taskResult.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Console.WriteLine($"Web client result{t.Exception}");
                else
                    Console.WriteLine($"Web client result{t.Result}");
            });

            GC.Collect();
            Thread.Sleep(10000);
            Console.WriteLine("Continuing on main thread.");
        }

    }

    internal class LogProcessor
    {
        private string logPath;

        private Regex _re = new Regex("GET /iangblog/(\\d\\d\\d\\d/\\d\\d/\\d\\d/[^ .]+)", RegexOptions.Compiled);

        private StreamReader _reader;

        private ConcurrentDictionary<string, int> _matchs = new ConcurrentDictionary<string, int>();

        public LogProcessor(string logPath)
        {
            if (logPath==null)
            {
                throw new ArgumentNullException("path");
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    _reader = new StreamReader(logPath);
                    FetchNextLine();
                }
                catch (Exception ex)
                { 
                }
            });   

           
        }

        private void FetchNextLine()
        {
            _reader.ReadLineAsync().ContinueWith(ProcessLine);
        }

        private void ProcessLine(Task<string> t)
        {
            string line = t.Result;
            if (line != null)
            {
                Match match = _re.Match(line);
                if (match.Success)
                {
                    string key = match.Groups[1].Value;
                    _matchs.AddOrUpdate(key, 1, (k, count) => count + 1);
                }
                FetchNextLine();
            }
            else
            {
                _reader.Close();
                foreach (var pair in _matchs.OrderByDescending(pair => pair.Value))
                {
                    Console.WriteLine($"{pair.Key} - {pair.Value}");
                }
            }
        }
    }
}
