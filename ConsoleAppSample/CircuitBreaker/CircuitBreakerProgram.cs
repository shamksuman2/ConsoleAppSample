using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.CircuitBreaker
{
    public class CircuitBreakerProgram
    {
        private static readonly CircuitBreaker myCircuitBreaker = new CircuitBreaker(3, TimeSpan.FromMinutes(15));

        public void Main()
        {
            Console.WriteLine(TestCircuitBreaker());
            Console.WriteLine(TestCircuitBreaker());
            Console.WriteLine(TestCircuitBreaker());
        }
        public string TestCircuitBreaker()
        {
            if (myCircuitBreaker.AttemptToCall(() => { DoSomething(); }).IsClosed)
            {
                return "Called Code.";
            }
            else
            {
                return "Too Many Failures, Resource is unavailable";
            }
        }

        private void DoSomething()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Do something");
                throw new NotImplementedException();
            }
        }
    }
}
