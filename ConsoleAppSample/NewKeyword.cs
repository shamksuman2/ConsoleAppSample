using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample
{
    public class NewKeyword
    {
        public void MainProgram()
        {
            DerivedClass objDerivedClass = new DerivedClass();
            objDerivedClass.Work();

            Baseclass objBaseclass = (Baseclass) objDerivedClass;

            objBaseclass.Work();
        }


    }

    public class Baseclass
    {
        public void Work() { Console.WriteLine("Base class work method."); }
        public void WorkDone() { Console.WriteLine("Base class work done method."); }
    }

    public class DerivedClass : Baseclass
    {
        public new void Work()
        {
            Console.WriteLine("Derived class work method.");
        }

        public new void WorkDone() 
        {
            Console.WriteLine("Derived class work done method.");
        }
    }
}
