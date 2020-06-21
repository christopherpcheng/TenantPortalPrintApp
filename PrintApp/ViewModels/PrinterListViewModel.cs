using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PrintApp.Models;
using PrintApp.Singleton;
using Serilog;

namespace PrintApp.ViewModels
{
    public class PrinterListViewModel : ViewModelBase
    {
        public ObservableCollection<PrinterItem> Printers { get; }
        public List<string> PrintersL { get; set; }

        public PrinterListViewModel(IEnumerable<PrinterItem> printers)
        {
            Printers = new ObservableCollection<PrinterItem>(printers);

            PrintersL = new List<string>();

            foreach (var each in Printers)
            {
                PrintersL.Add(each.PrinterName);
            }
            
            
        }

        public void PrintIt()
        {
            Globals.Log("Print!");
        }
    }


}
