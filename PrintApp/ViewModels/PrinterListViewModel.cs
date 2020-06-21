using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using PrintApp.Models;
using PrintApp.Singleton;
using ReactiveUI;

namespace PrintApp.ViewModels
{
    public class PrinterListViewModel : ViewModelBase
    {
        string selectedPrinterName;
        public string SelectedPrinterName 
        { 
            get => selectedPrinterName;
            set => this.RaiseAndSetIfChanged(ref selectedPrinterName, value);
        }


        public ObservableCollection<PrinterItem> Printers { get; }
        public List<string> PrintersL { get; set; }

        public ReactiveCommand<Unit, PrinterItem> PrintCommand { get; }
        public ReactiveCommand<Unit, Unit> PrintCommand2 { get; }

        public PrinterListViewModel(IEnumerable<PrinterItem> printers)
        {
            Printers = new ObservableCollection<PrinterItem>(printers);

            PrintersL = new List<string>();

            foreach (var each in Printers)
            {
                PrintersL.Add(each.PrinterName);
            }

            PrintCommand = ReactiveCommand.Create(
                () => new PrinterItem { PrinterName = SelectedPrinterName }
                );

            PrintCommand2 = ReactiveCommand.Create(() => { });

            Observable.Merge(
                PrintCommand,
                PrintCommand2.Select(_ => (PrinterItem)null))
                .Take(1)
                .Subscribe(model =>
                {
                    if (model != null)
                    {
                        Globals.Log("Print!");
                        Globals.Log($"Printer Selected: {SelectedPrinterName}");

                        Globals.Log($"Got:{model.PrinterName}");

                    }
                }

                );


        }

        public void PrintIt()
        {
            Globals.Log("Print!");
            Globals.Log($"Printer Selected: {SelectedPrinterName}");
            

        }
    }


}
