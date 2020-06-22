﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PrintApp.Services;
using PrintApp.Singleton;
using PrintApp.ViewModels;
using PrintApp.Views;
using System;
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

            if (Globals.IsWindows()) ConsoleAllocator.ShowConsoleWindow();

            Globals.Log($"Start {Globals.GetBuildDate(Assembly.GetExecutingAssembly()).ToString()}");
            PrepFileURL();
            FileTools.Instance.ProcessLink(Globals.URLToFile);




        }

        private void PrepFileURL()
        {
            string[] args = Environment.GetCommandLineArgs();

            try
            {
                int i = 0;
                foreach (var param in args)
                {
                    if (i == args.Length - 1)
                    {
                        Globals.Log($"DEBUG ARGS({i}): {param}");
                        Globals.URLToFile = param;
                        
                    }
                    i++;

                }
            }
            catch (Exception e)
            {
                Globals.Log($"Exception: {e.Message}");
            }


        }
    }
}
