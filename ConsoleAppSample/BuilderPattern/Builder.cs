using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppSample.BuilderPattern
{
    public abstract class ReportBuilder
    {
        protected clsReport objReport;
        public abstract void SetReportType();
        public abstract void SetReportHeader();
        public abstract void SetReportFooter();

        public void CreateNewReport()
        {
            objReport = new clsReport();
        }

        public clsReport GetReport()
        {
            return objReport;
        }
    }

    public class ReportPDF : ReportBuilder
    {
        public override void SetReportType()
        {
            objReport.strReportType = "PDF";
        }

        public override void SetReportHeader()
        {
            objReport.SetReportHeader("XYZ pvt. ltd.");
        }

        public override void SetReportFooter()
        {
            objReport.SetReportFooter("Thanks");
        }
    }

    public class ReportExcel : ReportBuilder
    {
        public override void SetReportType()
        {
            objReport.strReportType = "Excel";
        }

        public override void SetReportHeader()
        {
            objReport.SetReportHeader("XYZ pvt. ltd.");
        }

        public override void SetReportFooter()
        {
            objReport.SetReportFooter("Thanks");
        }
    }

}
