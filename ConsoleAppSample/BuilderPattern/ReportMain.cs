using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.BuilderPattern
{
    public class BuilderPatternMain
    {
        public void Main()
        {
            clsReport objReport;
            Director objDirector = new Director();

            ReportPDF objReportPdf = new ReportPDF();

            objReport = objDirector.MakeReport(objReportPdf);
        }

    }
}
