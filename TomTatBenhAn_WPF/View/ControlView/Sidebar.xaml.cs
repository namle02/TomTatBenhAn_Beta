using System.Windows;
using System.Windows.Controls;

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

        //private void OpenReport_Click(object sender, RoutedEventArgs e)
        //{
        //    // Lấy MainViewModel từ DataContext của MainWindow
        //    var mainWindow = Application.Current.MainWindow as MainWindow;
        //    var mainVM = mainWindow?.DataContext as MainViewModel;

        //    // Lấy ContentViewModel
        //    var contentVM = mainVM?.ContentVM;

        //    if (contentVM == null || contentVM.PatientInfo == null)
        //    {
        //        MessageBox.Show("Không có dữ liệu bệnh nhân để xuất báo cáo.");
        //        return;
        //    }

        //    string aiTomTatQuaTrinhBenhLy = contentVM.Ai_TomTatQuaTrinhBenhLy;
        //    string aiDauHieuLamSang = contentVM.Ai_DauHieuLamSang;
        //    string aiKQXN = contentVM.AiResultKQXN;
        //    string aiDienBien = contentVM.AiResultDienBien;
        //    string tinhTrangRaVien = contentVM.TinhTrangRaVien;
        //    string huongDieuTriTiepTheo = contentVM.HuongDieuTriTiepTheo;


        //    // Truyền cả tất cả vào ReportPage
        //    var reportWindow = new ReportPage(contentVM.PatientInfo,
        //        aiTomTatQuaTrinhBenhLy,
        //         aiDauHieuLamSang,
        //         contentVM.HanhChinhInfo,
        //        contentVM.ChiTietBenhAnInfo,
        //        aiKQXN,
        //        aiDienBien,
        //        contentVM.CheckBoxInfo,
        //        contentVM.ChuanDoanInfo,
        //        tinhTrangRaVien,
        //        huongDieuTriTiepTheo);
        //    reportWindow.ShowDialog();
        //}

    }
}
