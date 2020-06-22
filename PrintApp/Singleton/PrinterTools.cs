using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


namespace PrintApp.Singleton
{
    public class PrinterTools
    {
        private static void PrintPDFWindows(string printerName, string tmpFilename)
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

        private static void PrintPDFCLI(string printerName, string tmpFilename)
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "lpr";
                process.StartInfo.Arguments = $"-P \"{printerName}\" {tmpFilename}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, data) => {
                    Globals.Log(data.Data);
                };
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += (sender, data) => {
                    Globals.Log(data.Data);
                };
                process.Start();

            }
            catch (Exception e)
            {
                Globals.Log($"PDFError: {e.Message}");
            }

        }

        public static void PrintPDF(string printerName, string tmpFilename)
        {
            if (Globals.IsWindows())
            {
                PrintPDFWindows(printerName, tmpFilename);
            }
            else if ((Globals.IsOSX()) || (Globals.IsLinux())) 
            {
                PrintPDFCLI(printerName, tmpFilename);
            }
        }

        public static string GetDefaultPrinter()
        {
            string result = string.Empty;
            if (Globals.IsWindows())
            {
                PdfDocument pdf = new PdfDocument();
                result = pdf.PrintSettings.PrinterName;
            }
            return result;

        }


    }
}
