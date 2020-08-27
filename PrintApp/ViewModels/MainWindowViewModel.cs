using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using PrintApp.Services;
using PrintApp.Singleton;
using ReactiveUI;

namespace PrintApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase contentVM;
        public ReactiveCommand<Unit,Unit> CloseCommand;

        public ViewModelBase ContentVM
        {
            get => contentVM;
            private set => this.RaiseAndSetIfChanged(ref contentVM, value);
        }
        public PrinterListViewModel PrinterListVM { get; }

        public MainWindowViewModel(PrinterDatabase pdb)
        {
            ContentVM = PrinterListVM = new PrinterListViewModel(pdb.GetItems());
            CloseCommand = ReactiveCommand.Create(() => CloseMainViewModelCommand()); ;
        }


        public void CloseMainViewModelCommand()
        {
            try
            {
                Globals.DestroyFile();
                Globals.Log("BYE!!!!!");
                //Console.ReadLine();
            }
            catch
            {

            }
            finally
            {

            }
            
        }

    }
}
