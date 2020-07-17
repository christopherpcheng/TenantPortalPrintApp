using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
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

            if (!FileTools.Instance.ProcessLink(Globals.URLToFile))
            {
                Globals.Log($"ERROR: ProcessLink failed {Globals.URLToFile}");
//                Console.ReadLine();
                Environment.Exit(1);
            }

           
            if (Globals.FileToPrint == string.Empty)
            {
                Globals.OK = false;
                Globals.Message = "COULD NOT RETRIEVE BILLING STATEMENT";
                /*
                var messageBoxCustomWindow = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    Style = Style.Windows,
                    ContentMessage = "ERROR: Could not retrieve billing statement",
                    Icon = Icon.Forbidden,
                    ShowInCenter = false,
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "OK", Type = ButtonType.Colored }
                    }
                });
                messageBoxCustomWindow.Show();
                */
                

                //Globals.Log($"ERROR: No file downloaded");
                //Console.ReadKey();
                //Environment.Exit(1);


            }
            else
            {
                HTTPTools.ParseQueryString(Globals.URLToFile);

                if (Globals.ParamVersion != Assembly.GetEntryAssembly()
                        .GetCustomAttribute<VersionAttribute>()
                        .AppVersion.Replace("\"", ""))
                {
                    Globals.OK = false;
                    Globals.Message = "OUTDATED VERSION. PLEASE UPDATE";
                }

            }



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
                if (Globals.IsWindows())
                {
                    expectedCount = 2;
                }
                else if (Globals.IsOSX())
                {
                    expectedCount = 2;

                }
#endif
#if DEBUG                
                expectedCount = args.Length;
#endif
                

                if (args.Length <= expectedCount)
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
                    Globals.Log($"ERR: Exit due to argcount {args.Length} vs expected: {expectedCount}");
                    //Console.ReadLine();
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
