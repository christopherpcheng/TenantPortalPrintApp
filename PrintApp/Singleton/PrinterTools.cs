using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Text;


namespace PrintApp.Singleton
{
    public class PrinterTools
    {
        public static void PrintPDF(string printerName, string tmpFilename)
        {
            try
            {
                PdfDocument pdf = new PdfDocument();
                pdf.PrintSettings.PrinterName = printerName;
                Globals.Log($"PDF: Loading file: {tmpFilename}");
                pdf.LoadFromFile(tmpFilename);
                Globals.Log($"PDF: Printing to: {printerName}");
                pdf.PrintSettings.Color = false;
                pdf.Print();
                Globals.Log($"PDF: Done!");
            }
            catch (Exception e)
            {
                Globals.Log($"PDFError: {e.Message}");
            }

        }

        public static string GetDefaultPrinter()
        {
            PdfDocument pdf = new PdfDocument();
            return pdf.PrintSettings.PrinterName;
        }
    }
}
