using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PrintApp.Services;
using PrintApp.Singleton;
using PrintApp.ViewModels;
using PrintApp.Views;

namespace PrintApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var pdb = new PrinterDatabase();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(pdb),
                };
            }
            ConsoleAllocator.ShowConsoleWindow();
            Globals.Log("Start");

            base.OnFrameworkInitializationCompleted();
        }
    }
}
