using PdfiumPrinter;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

                Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
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

        private static void PrintPDFWindows2(string printerName, string tmpFilename)
        {
            Globals.Log($"PRINTING windows v2! {printerName} and {tmpFilename}");
            try
            {
                Globals.Log($"PDF2: Loading file: {tmpFilename}");
                Globals.Log($"PDF2: Printing to: {printerName}");


                var printer = new PdfPrinter(printerName);
                printer.PageSettings.Color = false;
                printer.Print(tmpFilename);
                //printer.Print("C:/Temp/0000000208.pdf");


                Globals.Log($"PDF2: Done!");
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
                int timeOut = 60000; //5 seconds = 5000

                var process = new Process();
                process.StartInfo.FileName = "lpr";
                process.StartInfo.Arguments = $"-P \"{printerName}\"  -T \"{Globals.PRINTJOB_NAME}\" \"{tmpFilename}\"";
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

                //process.WaitForInputIdle();
                process.WaitForExit(timeOut);
                if (process.HasExited == false)
                {
                    if (process.Responding)
                        process.CloseMainWindow();
                    else
                        process.Kill();
                }
                Globals.Log("PrintCLI Continue");

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
                PrintPDFWindows2(printerName, tmpFilename);
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
                Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                result = pdf.PrintSettings.PrinterName;
            }
            return result;

        }


    }
}
