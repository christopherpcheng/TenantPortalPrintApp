using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;

namespace PrintApp.Views
{
    public class PrinterListView : UserControl
    {
        public PrinterListView()
        {
            this.InitializeComponent();
            
            //var fontComboBox = this.Find<ComboBox>("fontComboBox");
            //fontComboBox.Items = new List<string> { "a","b","c"};
            
            //fontComboBox.SelectedIndex = 0;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
