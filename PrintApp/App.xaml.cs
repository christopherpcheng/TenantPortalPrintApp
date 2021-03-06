﻿using Avalonia;
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
using System.Reflection;

namespace PrintApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            
#if DEBUG
            InitLogging();
            if (Globals.IsWindows()) ConsoleAllocator.ShowConsoleWindow();
#endif

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



            Globals.Log($"Start {Globals.GetBuildDate(Assembly.GetExecutingAssembly())}");


            Globals.URLParamter = PrepFileURL();

            if (!FileTools.Instance.ProcessLink(Globals.URLParamter)) //sets URLToFile
            {
                Globals.Log($"ERROR: ProcessLink failed {Globals.URLToFile}");

                Globals.OK = false;
                Globals.Message = "ERROR:"+ Globals.Message;
            }

            string res = HTTPTools.ParseQueryString(Globals.URLParamter);
            if (res != string.Empty)
            {
                Globals.OK = false;
                Globals.Message = "PARSE ERROR:" + res;
            }
            else if (Globals.ParamVersion != Assembly.GetEntryAssembly()
                    .GetCustomAttribute<VersionAttribute>()
                    .AppVersion.Replace("\"", ""))
            {
                string myVersion = Assembly.GetEntryAssembly()
                    .GetCustomAttribute<VersionAttribute>()
                    .AppVersion.Replace("\"", "");
                Globals.OK = false;
                Globals.Message = $"OUTDATED VERSION. PLEASE UPDATE:{myVersion} -> {Globals.ParamVersion}";
            }

            if (Globals.OK && (!FileTools.Instance.GetPDF(Globals.URLToFile)))
            {
                Globals.Log($"ERROR: GetPDF failed {Globals.URLToFile}");
                Globals.OK = false;
                Globals.Message = "ERROR:" + Globals.Message;
            }

            if ((Globals.FileToPrint == string.Empty)&&(Globals.OK))
            {
                Globals.OK = false;
                Globals.Message = "COULD NOT RETRIEVE BILLING STATEMENT(2)";
            }



        }


        public void InitLogging()
        {
            string logpath = string.Empty;
            if (Globals.IsWindows())
            {
                logpath = Globals.LOGPATH_DEBUG_WIN;
            } else if (Globals.IsOSX())
            {
                logpath = Globals.LOGPATH_DEBUG_OSX;

            }
            Log.Logger = new LoggerConfiguration().CreateLogger();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logpath, rollingInterval: RollingInterval.Day)
                //.WriteTo.File("rtp-log.txt",
                //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Debug("STARTING!");
        }


        private string SanitizeInput(string param)
        {
            return param;
        }

        private string PrepFileURL()
        {
            string[] args = Environment.GetCommandLineArgs();

            try
            {
                int i = 0;
                int expectedCount = 0;

#if DEBUG
                expectedCount = args.Length;
#else
                if (Globals.IsWindows())
                {
                    expectedCount = 2;
                }
                else if (Globals.IsOSX())
                {
                    expectedCount = 2;

                }
#endif


                if (args.Length <= expectedCount)
                {
                    foreach (var param in args)
                    {
                        if (i == args.Length - 1)
                        {
                            string p = SanitizeInput(param);
                            //p = "rtenant-portal://mobilegroupinc.com/resources/pdf/0000000208.pdf?tenant_id=1&ts_invoice_no=2&v=0.2.0";
                            Globals.Log($"DEBUG ARGS({i}): {p}");
                            return p;

                        }
                        i++;

                    }
                    return string.Empty;
                }
                else
                {
                    Globals.Log($"ERR: Exit due to argcount {args.Length} vs expected: {expectedCount}");
                    //Console.ReadLine();
                    Environment.Exit(1);
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                Globals.Log($"Exception: {e.Message}");
                return string.Empty;
            }


        }
    }
}
