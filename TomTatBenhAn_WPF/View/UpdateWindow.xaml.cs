using System.Windows;
using TomTatBenhAn_WPF.ViewModel.PageViewModel;

namespace TomTatBenhAn_WPF.View
{
    public partial class UpdateWindow : Window
    {
        public UpdateWindow(UpdateViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

