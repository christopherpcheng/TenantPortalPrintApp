using System;
using System.Collections.Generic;
using System.Text;
using PrintApp.Models;

namespace PrintApp.Services
{
    public class PrinterDatabase
    {
        public IEnumerable<PrinterItem> GetItems() => new[]
        {
            new PrinterItem {PrinterName ="Printer 1", ID = "1"},
            new PrinterItem {PrinterName ="Printer 2", ID = "2"},
            new PrinterItem {PrinterName ="Printer 3", ID = "3"},
        };
    }
}
