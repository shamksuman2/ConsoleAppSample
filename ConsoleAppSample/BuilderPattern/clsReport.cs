using System;
using System.Collections;

namespace ConsoleAppSample.BuilderPattern
{
    public class clsReport
    {
        public string strReportType;
        private ArrayList objHeader= new ArrayList();
        private ArrayList objFooter = new ArrayList();

        public void SetReportHeader(string strData)
        {
            objHeader.Add(strData);
        }

        public void SetReportFooter(string strData)
        {
            objFooter.Add(strData);
        }

        public void DisplayReport()
        {
            Console.WriteLine(" ------------------------------------");
            Console.WriteLine($"Report Type {strReportType}");
            foreach (string header in objHeader)
            {
                Console.WriteLine(header);
            }

            Console.WriteLine(" ------------------------------------");
            Console.WriteLine(" ------------------------------------");
            Console.WriteLine(" ------------------------------------");

            foreach (string footer in objFooter)
            {
                Console.WriteLine(footer);
            }

            Console.WriteLine(" ------------------------------------");

        }
    }
}