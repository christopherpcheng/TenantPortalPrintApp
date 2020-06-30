using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PrintApp.Services;
using PrintApp.Singleton;
using PrintApp.ViewModels;
using PrintApp.Views;
using Serilog;
using System;
using System.Diagnostics;
using System.Reflection;

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
            base.OnFrameworkInitializationCompleted();


#if DEBUG
            if (Globals.IsWindows()) ConsoleAllocator.ShowConsoleWindow();
#endif

//            PrinterTools.PrintPDFCLI2("","");

//            InitLogging();

            Globals.Log($"Start {Globals.GetBuildDate(Assembly.GetExecutingAssembly())}");
            PrepFileURL();
            FileTools.Instance.ProcessLink(Globals.URLToFile);

//            PrinterTools.PrintPDFCLI2("", "");



        }

        public void InitLogging()
        {
            Log.Logger = new LoggerConfiguration().CreateLogger();
            Log.Information("No one listens to me!");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt",rollingInterval:RollingInterval.Day)
                //.WriteTo.File("rtp-log.txt",
                //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Information("Ah, there you are!");
        }

        private string SanitizeInput(string param)
        {
            return param;
        }

        private void PrepFileURL()
        {
            string[] args = Environment.GetCommandLineArgs();

            try
            {
                int i = 0;
                int expectedCount = 0;
#if !DEBUG
//                expectedCount = 1;
#endif
#if DEBUG                
//                expectedCount = 2;
#endif
                if (args.Length >= expectedCount)
                {
                    foreach (var param in args)
                    {
                        if (i == args.Length - 1)
                        {
                            string p = SanitizeInput(param);
                            Globals.Log($"DEBUG ARGS({i}): {p}");
                            Globals.URLToFile = p;

                        }
                        i++;

                    }
                }
                else
                {
                    Environment.Exit(1);
                }
            }
            catch (Exception e)
            {
                Globals.Log($"Exception: {e.Message}");
            }


        }
    }
}
