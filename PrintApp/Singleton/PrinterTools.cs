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
            Globals.Log($"PRINTING windows! {printerName} and {tmpFilename}");
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
            Globals.Log($"PRINTING CLI! {printerName} and {tmpFilename}");
            try
            {
                var process = new Process();
                process.StartInfo.FileName = "lpr";
                process.StartInfo.Arguments = $"-P \"{printerName}\"  -T \"Billing\" \"{tmpFilename}\"";
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

        public static void PrintPDFCLI3(string printerName, string tmpFilename)
        {

            string pn = "Epson_L565_Series";
            string tn = tmpFilename;

            Console.WriteLine($"PrintCLI3 {pn} and {tn}");

            try
            {
                var process = new Process();

                process.StartInfo.FileName = "lpr";
                process.StartInfo.Arguments = $"-P \"{pn}\" {tn}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, data) => {
                    Console.WriteLine(data.Data);
                };
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += (sender, data) => {
                    Console.WriteLine(data.Data);
                };
                process.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine($"PDFError: {e.Message}");
            }

        }

        public static void PrintPDFCLI2(string printerName, string tmpFilename)
        {
            Console.WriteLine("Hello World!");

            string pn = "Epson_L565_Series";
            string tn = "test.pdf";

            Console.WriteLine($"PrintCLI2 {pn} and {tn}");

            try
            {
                var process = new Process();

                process.StartInfo.FileName = "lpr";
                process.StartInfo.Arguments = $"-P \"{pn}\" {tn}";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, data) => {
                    Console.WriteLine(data.Data);
                };
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += (sender, data) => {
                    Console.WriteLine(data.Data);
                };
                process.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine($"PDFError: {e.Message}");
            }

        }


        public static void PrintPDF(string printerName, string tmpFilename)
        {
            Globals.Log($"About to Print: {tmpFilename} to {printerName} ");
            if (Globals.IsWindows())
            {
                Globals.Log($"Windows Print");
                PrintPDFWindows(printerName, tmpFilename);
            }
            else if ((Globals.IsOSX()) || (Globals.IsLinux())) 
            {
                Globals.Log($"CLI Print");
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
