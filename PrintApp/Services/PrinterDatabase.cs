using System;
using System.Collections.Generic;
using System.Text;
using PrintApp.Models;
using PrintApp.Singleton;

namespace PrintApp.Services
{
    public class PrinterDatabase
    {
        /*
        public IEnumerable<PrinterItem> GetItems() => new[]
        {
            new PrinterItem {PrinterName ="Printer 1"},
            new PrinterItem {PrinterName ="Printer 2"},
            new PrinterItem {PrinterName ="Printer 3"},
        };
        */
        public IEnumerable<PrinterItem> GetItems()
        {
            return PopulatePrinter();
        }

        private IEnumerable<PrinterItem> PopulatePrinter()
        {
            List<PrinterItem> listPrinters = new List<PrinterItem>();

            foreach (string printerName in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                bool goodPrinter = true;
                foreach (string badprinter in Globals.BADPRINTERS)
                {
                    if (printerName.ToLower().Contains(badprinter))
                    {
                        goodPrinter = false;
                        break;
                    }
                }
                if (goodPrinter)
                {
                    listPrinters.Add(new PrinterItem { PrinterName = printerName });
                }
            }
            
            return listPrinters;

        }
    }
}
