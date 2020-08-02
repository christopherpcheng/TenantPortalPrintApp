using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using PrintApp.Models;
using PrintApp.Singleton;
using ReactiveUI;


namespace PrintApp.ViewModels
{
    public class PrinterListViewModel : ViewModelBase
    {
        string version;
        public string Version { 
            get => version; 
            set => this.RaiseAndSetIfChanged(ref version, value);
        }

        string message;
        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }

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

            Version = "Version: "+Assembly.GetEntryAssembly()
                .GetCustomAttribute<VersionAttribute>()
                .AppVersion.Replace("\"","");
/*
            if (Globals.FileToPrint == string.Empty)
            {
                Message = "FILE RETRIEVAL FAILED";
            }
*/
            /*
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
            */

            Printers = new ObservableCollection<PrinterItem>(printers);

            PrintersL = new List<string>();

            foreach (var each in Printers)
            {
                PrintersL.Add(each.PrinterName);
            }

            var okEnabled = this.WhenAnyValue(
                x => x.SelectedName,
                x => !string.IsNullOrWhiteSpace(x)
                );


            PrintCommand = ReactiveCommand.Create(
                () => new PrinterItem { PrinterName = SelectedPrinterName },
                okEnabled
                );



            PrintCommand
            .Take(1)
            .Subscribe(async model => 
            {
                bool success = false;

                if ((model != null)&&(Globals.OK))
                {
                    Globals.Log("Checking if printer is active");

                    if (PrinterTools.CheckPrinter(SelectedPrinterName))
                    {
                        Globals.Log("Printer is active");

                        Globals.Log("Calling Tagging API");
                        if (HTTPTools.CallTaggingAPI(Globals.ParamValue1, Globals.ParamValue2))
                        {
                            Globals.Log("Print!");
                            Globals.Log($"Printer Selected: {SelectedPrinterName}");

                            Globals.Log($"Got:{model.PrinterName}");

                            //PrinterTools.PrintPDFCLI2("", "");

                            PrinterTools.PrintPDF(SelectedPrinterName, Globals.FileToPrint);

                            success = true;

                            //PrinterTools.PrintPDFCLI3("", Globals.FileToPrint);
                            //Version = "SUCCESS!";

                            /*
                            var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = ButtonEnum.Ok,
                                Icon = Icon.Info,
                                ContentTitle = "Success",
                                ContentMessage = "Print OK",
                                Style = Style.Windows,
                                CanResize = false,
                                ShowInCenter = true
                            });
                            var res = msBoxStandardWindow.Show();
                            */

                        }
                        else
                        {
                            Globals.OK = false;
                            Message = "FAILED TO UPDATE STATUS!";
                            /*
                            var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams
                            {
                                ButtnoDefinitions = ButtonEnum.Ok,
                                Icon = Icon.Error,
                                ContentTitle = "Failed!",
                                ContentMessage = "Print Failed",
                                Style = Style.Windows,
                                CanResize = false,
                                ShowInCenter = true
                            });
                            var res = msBoxStandardWindow.Show();
                            */
                        }


                        try
                        {
                            //   Message = "Cleanup";
                            Globals.Log("Cleanup");
                            File.Delete(Globals.FileToPrint);

                        }
                        catch
                        {
                            Globals.Log($"FireERR: Could not delete file {Globals.FileToPrint}");
                        }
                        Globals.Log("DONE");
                        //Console.ReadLine();

                        if (success)
                        {
                            Message = "Billing Statement successfully printed";
                            await Task.Delay(Globals.SLEEPTIMER);
                            Environment.Exit(0);
                        }

                    }
                    else
                    {
                        Message = $"PRINTER PROBLEM: {Globals.Message}";
                    }

                }
                else 
                {
                    Message = Globals.Message;
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

            //SelectedIndex = -1;
            SelectedIndex = PrintersL.IndexOf(PrinterTools.GetDefaultPrinter());



        }

        //If I just wanted to directly Bind this as a Command to the button in XAML 
        //without mucking with observable streams and stuff above
        public void PrintIt()
        {
            Globals.Log("Print No Nonsense!");
            Globals.Log($"Printer Selected: {SelectedPrinterName}");
            //PrinterTools.PrintPDFCLI("", "");

        }

    }


}
