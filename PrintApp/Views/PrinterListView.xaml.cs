using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PrintApp.Views
{
    public class PrinterListView : UserControl
    {
        public PrinterListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
