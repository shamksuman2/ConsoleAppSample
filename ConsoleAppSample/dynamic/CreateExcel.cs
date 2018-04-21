using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleAppSample.dynamic
{
    public class CreateExcel
    {
        public void CreateExcelFile()
        {
            Type excelType = Type.GetTypeFromProgID("Excel.Application", true);
            dynamic excel = Activator.CreateInstance(excelType);
            excel.Visible = true;
            excel.WorkBooks.Add();

            dynamic defaultWorksheet = excel.ActiveSheet;
            defaultWorksheet.Cells[1, "A"] = "This is the Name Column";
            defaultWorksheet.Column[1].AutoFit();
        }

        public void CreateExcelFileFromJson()
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(currentDir);
            dynamic jsonData = JsonConvert.DeserializeObject(File.ReadAllText($"{directory.FullName}\\dynamic\\Customers.json"));

            dynamic excel = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application", true));
            excel.Visible = true;
            excel.WorkBooks.Add();

            dynamic defaultWorksheet = excel.ActiveSheet;

            int currentRow = 1;
            foreach (dynamic customer in jsonData.Customers)
            {
                defaultWorksheet.Cells[currentRow, "A"] = customer.FirstName;
                defaultWorksheet.Cells[currentRow, "B"] = customer.LastName;
                currentRow ++;
            }
            defaultWorksheet.Column[1].AutoFit();
        }

    }
}
