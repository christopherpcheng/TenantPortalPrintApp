using System;
using System.Collections.Generic;
using System.Text;
using PrintApp.Services;
using ReactiveUI;

namespace PrintApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase contentVM;
        public ViewModelBase ContentVM
        {
            get => contentVM;
            private set => this.RaiseAndSetIfChanged(ref contentVM, value);
        }
        public PrinterListViewModel PrinterListVM { get; }

        public MainWindowViewModel(PrinterDatabase pdb)
        {
            ContentVM = PrinterListVM = new PrinterListViewModel(pdb.GetItems());
        }
    
    }
}
