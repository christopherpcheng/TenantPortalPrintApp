//#define _OSX
//#undef _WINDOWS

using PdfiumPrinter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;

namespace PrintApp.Singleton
{
    public class PrinterTools
    {

        private static void PrintPDFWindows(string printerName, string tmpFilename)
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
                var printer = new PdfPrinter();
                result = printer.Settings.PrinterName;
            }
            return result;

        }

#if _WINDOWS
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int OpenPrinter(string pPrinterName, out IntPtr phPrinter, ref PRINTER_DEFAULTS pDefault);

        [DllImport("winspool.drv", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetPrinter(IntPtr hPrinter, Int32 dwLevel, IntPtr pPrinter, Int32 dwBuf, out Int32 dwNeeded);

        [DllImport("winspool.drv", SetLastError = true)]
        public static extern int ClosePrinter(IntPtr hPrinter);

        [StructLayout(LayoutKind.Sequential)]
        public struct PRINTER_DEFAULTS
        {
            public IntPtr pDatatype;
            public IntPtr pDevMode;
            public int DesiredAccess;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PRINTER_INFO_2
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pServerName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrinterName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pShareName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPortName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDriverName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pComment;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pLocation;

            public IntPtr pDevMode;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pSepFile;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pPrintProcessor;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pDatatype;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pParameters;

            public IntPtr pSecurityDescriptor;
            public uint Attributes;
            public uint Priority;
            public uint DefaultPriority;
            public uint StartTime;
            public uint UntilTime;
            public uint Status;
            public uint cJobs;
            public uint AveragePPM;
        }

        public static PRINTER_INFO_2? GetPrinterInfoRaw(String printerName)
        {
            IntPtr pHandle;
            PRINTER_DEFAULTS defaults = new PRINTER_DEFAULTS();
            PRINTER_INFO_2? Info2 = null;

            OpenPrinter(printerName, out pHandle, ref defaults);

            Int32 cbNeeded = 0;

            bool bRet = GetPrinter(pHandle, 2, IntPtr.Zero, 0, out cbNeeded);

            if (cbNeeded > 0)
            {
                IntPtr pAddr = Marshal.AllocHGlobal((int)cbNeeded);

                bRet = GetPrinter(pHandle, 2, pAddr, cbNeeded, out cbNeeded);

                if (bRet)
                {
                    Info2 = (PRINTER_INFO_2)Marshal.PtrToStructure(pAddr, typeof(PRINTER_INFO_2));
                }

                Marshal.FreeHGlobal(pAddr);
            }

            ClosePrinter(pHandle);

            return Info2;
        }


        public static bool CheckPrinterWindows(string printerName)
        {
            bool found = false;
            try
            {
                PRINTER_INFO_2? p = GetPrinterInfoRaw(printerName);
                if (p != null)
                {
                    if (Convert.ToBoolean(p.Value.Status & Globals.PRINTER_STATUS_OFFLINE))
                    {
                        Globals.Log("*OFFLINE*");
                        Globals.Message = "OFFLINE";
                    }
                    else if (Convert.ToBoolean(p.Value.Attributes & Globals.PRINTER_ATTRIBUTE_WORK_OFFLINE))
                    {
                        Globals.Log("*WORK OFFLINE*");
                        Globals.Message = "WORK OFFLINE MODE";
                    }
                    else if (Convert.ToBoolean(p.Value.Status & Globals.PRINTER_STATUS_ERROR))
                    {
                        Console.WriteLine("Is in ERROR state");
                        Globals.Message = "ERROR STATE";
                    }
                    else if (Convert.ToBoolean(p.Value.Status & Globals.PRINTER_STATUS_PAPER_JAM))
                    {
                        Console.WriteLine("Is in JAM state");
                        Globals.Message = "PAPER JAM";
                    }
                    else if(Convert.ToBoolean(p.Value.Status & Globals.PRINTER_STATUS_PAPER_OUT))
                    {
                        Console.WriteLine("Is in PAPER OUT state");
                        Globals.Message = "PAPER OUT";
                    }
                    else if(Convert.ToBoolean(p.Value.Status & Globals.PRINTER_STATUS_NOT_AVAILABLE))
                    {
                        Console.WriteLine("Is in NOT AVAILABLE state");
                        Globals.Message = "NOT AVAILABLE";
                    }
                    else if (Convert.ToBoolean(p.Value.Status & Globals.PRINTER_STATUS_NO_TONER))
                    {
                        Console.WriteLine("Is in NO TONER state");
                        Globals.Message = "NO INK/TONER";
                    }
                    else
                    {
                        found = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.Log("PInvoke error: "+ex.Message);

                found = true; //In event of exception, print nonetheless to prevent issue due to various things that can go wrong with this
            }

            return found;
        }
        /*
        public static bool CheckPrinterStatusWMI(string printerName)
        {
            bool found = false;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                     @"SELECT * FROM Win32_Printer WHERE Name = '" + printerName.Replace("\\", "\\\\") + "'");

                foreach (ManagementObject printObj in searcher.Get())
                {
                    if (printObj["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        Globals.Log("Your Plug-N-Play printer is not connected.");
                    }
                    else
                    {
                        found = true;
                    }
                }
            }
            catch (ManagementException e)
            {
                Globals.Log("An error occurred while querying for WMI data: " + e.Message);
                found = true; //if this complicated call fails, default to true to avoid trouble
            }
            return found;


        }
        */

        public static bool CheckPrinter(string printerName)
        {
            return CheckPrinterWindows(printerName);
            //return CheckPrinterWindows(printerName) && CheckPrinterStatusWMI(printerName);
        }


#endif

#if _OSX
        public static bool CheckPrinter(string printerName)
        {
            return CheckPrinterMacOS(printerName);

        }

        public static bool CheckPrinterMacOS(string printerName)
        {
            bool found = false;

            try
            {


                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {

                        FileName = "lpinfo",
                        Arguments = $"--include-schemes dnssd,usb -v",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                process.OutputDataReceived += (sender, data) =>
                {
                    Globals.Log($"O: {data.Data}");
                };
                process.ErrorDataReceived += (sender, data) =>
                {
                    Globals.Log($"E: {data.Data}");
                };
                process.Start();
                process.WaitForExit(Globals.PROCTIMER);
                
                /*
                while (!process.StandardError.EndOfStream)
                {
                    string line = process.StandardError.ReadLine();
                    Console.WriteLine($"WE: {line} --> {ParsePrinterString(line)}");
                }
                */
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = process.StandardOutput.ReadLine();
                    line = ParseMacOSPrinterString(line);
                    if (line == printerName)
                    {
                        Globals.Log($"FOUND printer {line} matching selected {printerName}");
                        found = true;
                    }
                    //Console.WriteLine($"WO: {line} --> {ParsePrinterString(line)}");
                    if (!found)
                    {
                        Globals.Log($"No Printer found {printerName}");
                        Globals.Message = "NOT FOUND";
                    }
                }
            }
            catch (Exception e)
            {
                Globals.Log($"CPM: {e.Message}");
                found = true; //In event of exception, print nonetheless to prevent issue due to various things that can go wrong with this
            }

            return found;
        }

        public static string ParseMacOSPrinterString(string printerString)
        {
            if (printerString.Contains(@"://"))
            {
                printerString = printerString.Split(new string[] { "://" }, 2, StringSplitOptions.None)[1];
                printerString = printerString.Split('?')[0];
                printerString = printerString.Replace("/", "_");
                printerString = printerString.Replace("%20", "_");
            }

            return printerString;
        }

#endif

    }
}
