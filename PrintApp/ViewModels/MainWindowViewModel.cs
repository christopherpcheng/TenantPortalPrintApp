using System;
using System.Collections.Generic;
using System.Text;
using PrintApp.Services;

namespace PrintApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public PrinterListViewModel PrinterListVM { get; }

        public MainWindowViewModel(PrinterDatabase pdb)
        {
            PrinterListVM = new PrinterListViewModel(pdb.GetItems());
        }
    
    }
}
