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

        private List<string> _PrintersL;
        public List<string> PrintersL 
        { 
            get => _PrintersL;
            set
            {
                _PrintersL = value;
            }
        }

        public ReactiveCommand<Unit, PrinterItem> PrintCommand { get; }
        public ReactiveCommand<Unit, string> PrintCommand2 { get; }


        public string TestM()
        {
            Globals.Log($"1:{SelectedPrinterName}");
            
            return "yay!";
        }
       

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



            PrintCommand
            .Take(2)
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


            PrintCommand2 = ReactiveCommand.Create(
                () => TestM()
                );  



            PrintCommand2
            .Take(2)
            .Subscribe(model =>
            {
                if (model != null)
                {
                    Globals.Log("Print2!");
                    Globals.Log($"Printer2 Selected: {SelectedPrinterName}");

                    Globals.Log($"Got2:{model}");

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
