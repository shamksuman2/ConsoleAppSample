using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.dynamic
{
    class DynamicGeneric
    {

        public void DynamicGenericProgram()
        {
            int int1 = 5, int2 = 2;
            Console.WriteLine($"Int overload {CommonMathDynamic.Add(int1, int2)}");

            double double1 = 5, double2 = 2;
            Console.WriteLine($"double overload {CommonMathDynamic.Add(double1, double2)}");

            Console.WriteLine($"double overload {CommonMathGenericDynamic.Add(double1, double2)}");

            short short1 = 5, short2 = 2;
            Console.WriteLine($"short overload {CommonMathGenericDynamic.Add(short1, short2)}");


        }
    }
}
