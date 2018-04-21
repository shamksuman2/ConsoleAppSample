using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.BuilderPattern
{
    public class Director
    {
        public clsReport MakeReport(ReportBuilder objBuilder)
        {
            objBuilder.CreateNewReport();
            objBuilder.SetReportType();
            objBuilder.SetReportHeader();
            objBuilder.SetReportFooter();
            return objBuilder.GetReport();

        }
    }
}
