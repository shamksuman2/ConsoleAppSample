using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.dynamic
{
    public static class CommonMathDynamic
    {
        public static dynamic Add(dynamic a, dynamic b)
        {
            dynamic result = a + b;
            return result;
        }
    }

    public static class CommonMathGenericDynamic
    {
        public static T Add<T>(T a, T b)
        {
            dynamic result = (dynamic)a + b;
            return (T)result;
        }
    }
}
