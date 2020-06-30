using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using Avalonia;
using PrintApp.Models;
using PrintApp.Singleton;
using ReactiveUI;

namespace PrintApp.ViewModels
{
    public class PrinterListViewModel : ViewModelBase
    {
        public string Version { get; set; }

        public int SelectedIndex { get; set; } = 0;

        string selectedPrinterName;
        public string SelectedPrinterName //Bound to XAML to trigger on selects
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
                if (_PrintersL != value)
                {
                    _PrintersL = value;
                    //You can add stuff here to trigger on sets
                }
            }
        }

        private string _selectedName;
        public string  SelectedName
        {
            get => _selectedName;
            private set
            {
                SelectedPrinterName = value;
                this.RaiseAndSetIfChanged(ref _selectedName, value);
            }
        }

        public ReactiveCommand<Unit, PrinterItem> PrintCommand { get; } //object return
        public ReactiveCommand<Unit, string> PrintCommand2 { get; } //string return sample

        //Put stuff needed to return the result/output of return of button click command here
        public string TestM()
        {
            Globals.Log($"TestM Func:{SelectedPrinterName}");
            
            return "yay!";
        }
       

        public PrinterListViewModel(IEnumerable<PrinterItem> printers)
        {
            //Version = "Build:"+Globals.GetBuildDate(Assembly.GetExecutingAssembly()).ToString();
            
            try
            {
                string[] args = Environment.GetCommandLineArgs();
                foreach (var param in args)
                {
                    Version = Version + param +" || ";
                }
                
            }
            catch(Exception ex)
            {
                Version = "Err:"+ex.Message;
            }


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
            .Take(1)
            .Subscribe(model =>
            {
                /*
                Version = "Test" + Globals.URLToFile;
                Console.WriteLine("WTF"+Globals.URLToFile);
                Console.ReadLine();
                Environment.Exit(1);
                */

                if (model != null)
                {
                    Globals.Log("Print!");
                    Globals.Log($"Printer Selected: {SelectedPrinterName}");

                    Globals.Log($"Got:{model.PrinterName}");

                    //PrinterTools.PrintPDFCLI2("", "");

                    PrinterTools.PrintPDF(SelectedPrinterName, Globals.FileToPrint);

                    //PrinterTools.PrintPDFCLI3("", Globals.FileToPrint);

                    try
                    {
                        Globals.Log("Cleanup");
                        File.Delete(Globals.FileToPrint);

                    }
                    catch 
                    {
                        Globals.Log($"FireERR: Could not delete file {Globals.FileToPrint}");
                    }

                    Environment.Exit(0);


                }
            }

            );

            //Instead of jamming all the code here to generate the expected return
            //in anonymous func, you can reference a func directly
            PrintCommand2 = ReactiveCommand.Create(
                () => TestM()
                );  

            PrintCommand2
            .Take(2) //takes 2 clicks
            .Subscribe(model => //this is where the expected return generation happens
            {
                //do whatever after button click and you have the expected return already
                if (model != null)
                {
                    Globals.Log("Subs Print2!");
                    Globals.Log($"Printer2 Selected: {SelectedPrinterName}");

                    Globals.Log($"Got2:{model}");

                }
            }

            );

            SelectedIndex = PrintersL.IndexOf(PrinterTools.GetDefaultPrinter());



        }

        //If I just wanted to directly Bind this as a Command to the button in XAML 
        //without mucking with observable streams and stuff above
        public void PrintIt()
        {
            Globals.Log("Print No Nonsense!");
            Globals.Log($"Printer Selected: {SelectedPrinterName}");
            PrinterTools.PrintPDFCLI2("", "");

        }

    }


}
