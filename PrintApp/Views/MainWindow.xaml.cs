using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PrintApp.ViewModels;
using System.ComponentModel;

namespace PrintApp.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Closing += OnWindowClosing;

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            var viewModel = (MainWindowViewModel)DataContext;
            viewModel.CloseMainViewModelCommand();
            
            //CloseMainViewModelCommand();
            
        }
        
    }
}
