using iTextSharp;

using iTextSharp.text;
using iTextSharp.text.pdf;
using static NPOI.HSSF.Util.HSSFColor;

namespace WordToPDFConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the path to the Word document.
            string inputFilePath = @"C:\mis\efocus meeting.doc";
            string outputFilePath = @"C:\mis\efocus meeting.pdf";
            System.IO.FileStream fs = new FileStream(inputFilePath, FileMode.Create);


            // Create a new PDF document.
            // Create an instance of the document class which represents the PDF document itself.  
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            // Create an instance to the PDF file by creating an instance of the PDF   
            // Writer class using the document and the filestrem in the constructor.  
            // Add meta information to the document  
            document.AddAuthor("Micke Blomquist");
            document.AddCreator("Sample application using iTextSharp");
            document.AddKeywords("PDF tutorial education");
            document.AddSubject("Document subject - Describing the steps creating a PDF document");
            document.AddTitle("The document title - PDF creation using iTextSharp");

            // Open the document to enable you to write to the document  
            document.Open();
            // Add a simple and wellknown phrase to the document in a flow layout manner  
            document.Add(new Paragraph("Hello World!"));
            // Close the document  
            document.Close();
            // Close the writer instance  
            //writer.Close();
            // Always close open filehandles explicity  
            fs.Close();
            // Display a message indicating that the conversion was successful.
            Console.WriteLine("The conversion was successful!");
        }
    }
}