using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Controls;
using TomTatBenhAn_WPF.Message;
using TomTatBenhAn_WPF.ViewModel;
using TomTatBenhAn_WPF.ViewModel.ControlViewModel;

namespace TomTatBenhAn_WPF.View.ControlView
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public Sidebar()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            // Lấy MainViewModel từ DataContext của MainWindow
            var mainWindow = Application.Current.MainWindow as MainWindow;
            var mainVM = mainWindow?.DataContext as MainViewModel;

            // Lấy ContentViewModel
            var contentVM = mainVM?.ContentVM; // vì ObservableProperty auto-gen property là ContentVM

            if (contentVM == null || contentVM.PatientInfo == null)
            {
                MessageBox.Show("Không có dữ liệu bệnh nhân để xuất báo cáo.");
                return;
            }

            string aiTomTatQuaTrinhBenhLy = contentVM.Ai_TomTatQuaTrinhBenhLy;
            string aiDauHieuLamSang = contentVM.Ai_DauHieuLamSang;

            // Truyền cả tất cả vào ReportPage
            var reportWindow = new ReportPage(contentVM.PatientInfo,
                aiTomTatQuaTrinhBenhLy,
                 aiDauHieuLamSang,
                 contentVM.HanhChinhInfo,
                contentVM.ChiTietBenhAnInfo,
               
                contentVM.ChuanDoanInfo);
            reportWindow.ShowDialog();
        }

    }
}
