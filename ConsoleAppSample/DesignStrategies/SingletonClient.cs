using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ConsoleAppSample.DesignStrategies
{
    [TestFixture]
    public class SingletonClient
    {
        public void UseSingleton()
        {
            Singleton s1 = Singleton.Instance;
            s1.DoSomething("S1");
            Singleton s2 = Singleton.Instance;
            s2.DoSomething("S2");
            Assert.AreSame(s1, s2);
        }
    }
}
