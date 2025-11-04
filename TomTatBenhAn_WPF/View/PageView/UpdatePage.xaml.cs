using System.Windows;
using System.Windows.Controls;

namespace TomTatBenhAn_WPF.View.PageView
{
    public partial class UpdatePage : UserControl
    {
        public UpdatePage()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Tìm window cha và đóng nó
            Window parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }
    }
}

