using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Scripting.Hosting.Shell;

namespace ConsoleAppSample
{
    public  class TryOutDictionary
    {
        private ConcurrentDictionary<int, string> commandList = new ConcurrentDictionary<int, string>();
        private Dictionary<int, string> dicItem = new Dictionary<int, string>();
        public void IterateConcurrentDictionalry ()
        {
            dicItem.Add(1, "a");
            dicItem.Add(2, "b");
            commandList.TryAdd(1, "A");
            foreach (var item in dicItem)
            {
                Console.WriteLine(item);
            }
            foreach (var command  in commandList)
            {
                Console.WriteLine(command);
            }
        }
    }

    public class TryOutGenericDelegate<T, TT> where T: class where TT : class
    {
        public void Sum<T, TT>(T i, TT j) { 
            Console.Write(i + ", " + j);
        }

        
    }

    public class TryOutGenericDelegateMain
    {
        public void Main()
        {
            var obj = new TryOutGenericDelegate<string, string>();
            obj.Sum<string, string>("1", "2");

        }
    }

    public class TryOutDelegate
    {
        public delegate void delegateMethod(int i, int j);
        public delegate void Print(int i, int j);
        public void Main() {
            delegateMethod method1 = Sum;
            method1(1, 2);

            delegateMethod method2 = Sum; method2(1, 2);
            Print prnt = delegate (int i, int j)
            {
               Console.WriteLine($"{i+j}");
            };

            Print a = (int i, int j) => { Console.WriteLine(i + j); };
            a(1, 2);
            
            ////////////Func delegate
            Func<int, int, int> sumFunc = Sum2;
            Console.WriteLine(sumFunc(1, 2));

            ////////////Action delegate
            Action<int, int> sumAction = Sum;
            sumAction(1,1);

            Predicate<int> predicate = TrySum;
        }

        public void Sum(int no1, int no2)
        {
        }
        public int Sum2(int no1, int no2)
        {
                return 0;
        }
        public bool TrySum(int no1)
        {
            return false;
        }
    }
    public static class TryOutExtension
    {
        public static int CharCount(this string input)
        {

            return input.Length;
        }
    }
}
