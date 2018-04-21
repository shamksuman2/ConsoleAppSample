using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace ConsoleAppSample
{

    public class DynamicProgram
    {
        public void ProgramMain()
        {

            //InvokeMethodUsingReflection();
            //InvokeMethodUsingDynamic();

            dynamic customer = new ExpandoObject();
            customer.ID = "42";
            Console.WriteLine("enter 'done' to complete adding property");

            string propertyName = GetPropertyName();

            while (propertyName!= "done")
            {
                string propertyValue = GetPropertyValue();

                customer.Count = (Func<int>)(() =>
                {
                    var c = (IDictionary<string, object>)customer;
                    return c.Count;
                });

                var cc = (IDictionary<string, object>)customer;

                cc.Add(propertyName, propertyValue);

                propertyName = GetPropertyName();
            }
            Console.WriteLine("\nCustomer properties");

            customer.print((Action)(()=>{
                foreach (KeyValuePair<string, object> item in customer)
                {
                    Console.WriteLine($"{item.Key} : {item.Value}");
                }
            }));

            
            Console.WriteLine("\n\nPress to exit...");
            //---------------limitaion -  static methods can not be called by dynamic variable.--------
            //dynamic gentry = "Gentry";
            //Console.WriteLine(gentry.AppendHello());

            //----------------------------------------
            //Customer cust = new Customer();

            //PropertyInfo firstNameProperty = typeof(Customer).GetProperty("FirstName");
            //foreach (var attribute in firstNameProperty.CustomAttributes)
            //{
            //    Console.WriteLine(attribute);
            //}
            //Console.WriteLine($"{firstNameProperty.PropertyType} FirstName");

            //PropertyInfo secondNameProperty = typeof(Customer).GetProperty("LastName");
            //foreach (var attribute in secondNameProperty.CustomAttributes)
            //{
            //    Console.WriteLine(attribute);
            //}
            //Console.WriteLine($"{secondNameProperty.PropertyType} LastName");

            

            //--------------------
            //int i = 42;
            //PrintMe(i);

            //dynamic d;
            //Console.WriteLine("Create [i]nt or [d]ouble");
            //ConsoleKeyInfo choice = Console.ReadKey(intercept: true);
            //if (choice.Key == ConsoleKey.I)
            //{
            //    d = 99;
            //}
            //else
            //{
            //    d = 55.5;
            //}
            //d = long.MaxValue;

            //PrintMe(d);

            Console.ReadLine();

        }

        private string GetPropertyValue()
        {
            Console.Write("enter attribute value");
            return Console.ReadLine();
        }

        private string GetPropertyName()
        {
            Console.Write("enter attribute name");
            return Console.ReadLine();
        }

        void PrintMe(int value)
        {
            Console.WriteLine($"PrintMe int called :{value}");
        }

        void PrintMe(long value)
        {
            Console.WriteLine($"PrintMe long called :{value}");
        }

        void PrintMe(dynamic value)
        {
            Console.WriteLine($"PrintMe dynamic called :with {value.GetType()} value: {value}");
        }

        private static void InvokeMethodUsingDynamic()
        {
            StringBuilder sb = new StringBuilder();

            ((dynamic)sb).AppendLine("Hello dynamic");

            Console.WriteLine(sb);
        }

        private static void InvokeMethodUsingReflection()
        {
            StringBuilder sb = new StringBuilder();
            sb.GetType().InvokeMember("AppendLine", BindingFlags.InvokeMethod, null, sb, new object[] { "Hello Reflection" });

            Console.WriteLine(sb);
        }


    }

    static class StringExtension
    {
        public static string AppendHello(this string s)
        {
            return $"Hello{s}";

        }
    }
    class Customer
    {
        public object FirstName { get; set; }
        public dynamic LastName { get; set; }
    }


}
