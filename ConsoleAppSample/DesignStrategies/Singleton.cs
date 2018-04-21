using System;

namespace ConsoleAppSample.DesignStrategies
{
    public sealed class Singleton
    {
        //public static readonly Lazy<Singleton> LazyInstance = new Lazy<Singleton>(() => new Singleton(), true);
        private static Singleton _instance;
        private static int _counter;
        private Singleton()
        {
            _counter++;
            Console.WriteLine($"Singleton instance count: {_counter.ToString()}");
        }

        public static Singleton Instance
        {
            get
            {
                return _instance = _instance ?? (_instance = new Singleton());
            }
        }

        public void DoSomething(string item)
        {
            Console.WriteLine(item);
        }

    }
}
